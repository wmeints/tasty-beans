using System.Diagnostics;
using System.Diagnostics.Metrics;
using Akka.Actor;
using Akka.Event;
using TastyBeans.Simulation.Domain.Aggregates.CustomerAggregate.Commands;
using TastyBeans.Simulation.Domain.Common;
using TastyBeans.Simulation.Domain.Services.Ratings;
using TastyBeans.Simulation.Domain.Services.ShippingInformation;
using TastyBeans.Simulation.Domain.Services.Subscriptions;

namespace TastyBeans.Simulation.Domain.Aggregates.CustomerAggregate;

public class Customer : ReceiveActor
{
    private readonly Random _random = new();
    private readonly Guid _customerId;
    private readonly CustomerProfile _customerProfile;
    private readonly IShippingInformation _shippingInformation;
    private readonly IRatings _ratings;
    private readonly ISubscriptions _subscriptions;

    private double _currentLoyaltyLevel;
    private double _currentDeliveryQualityLevel;

    private int _totalDeliveries;
    private int _deliveriesLost;
    private CircularBuffer<int> _previousRatings = new CircularBuffer<int>(3);


    public Customer(Guid customerId, CustomerProfile customerProfile, IShippingInformation shippingInformation,
        IRatings ratings, ISubscriptions subscriptions)
    {
        _customerId = customerId;
        _customerProfile = customerProfile;
        _shippingInformation = shippingInformation;
        _ratings = ratings;
        _subscriptions = subscriptions;

        _currentLoyaltyLevel = customerProfile.Loyality;
        _currentDeliveryQualityLevel = customerProfile.DeliveryQuality;

        ReceiveAsync<ShipmentDelivered>(async msg => await OnShipmentDelivered(msg.ShippingOrderId));
        ReceiveAsync<ShipmentLost>(async msg => await OnShipmentLost(msg.ShippingOrderId));

        // The customer gets all kinds of messages related to deliveries.
        // We'll handle them all with a generic handler if not handled above.
        // We've done this to make sure our logs don't overflow with unwanted errors.

        ReceiveAny(msg => { Sender.Tell(OK.Instance); });
    }

    public static Props Props(Guid customerId, CustomerProfile customerProfile,
        IShippingInformation shippingInformation, IRatings ratings, ISubscriptions subscriptions)
    {
        return new Props(
            type: typeof(Customer),
            supervisorStrategy: Akka.Actor.SupervisorStrategy.DefaultStrategy,
            args: new object[] { customerId, customerProfile, shippingInformation, ratings, subscriptions });
    }

    private async Task OnShipmentLost(Guid msgShippingOrderId)
    {
        Context.GetLogger().Info("Too bad, one of my shipments got lost!");

        using var activity = Activities.ShipmentLost();

        _deliveriesLost++;
        _totalDeliveries++;

        var deliveryQuality = (_totalDeliveries - _deliveriesLost) / _totalDeliveries;

        // When the delivery quality falls below the treshold, we calculate
        // the chance that the customer cancels based on their loyality level at this point.
        // If the loyality check fails, we cancel the subscription.
        if (deliveryQuality < _currentDeliveryQualityLevel && IsNoLongerLoyal())
        {
            Context.GetLogger().Warning("Customer is unhappy and no longer wishes to subscribe.");
            await _subscriptions.CancelSubscriptionAsync(_customerId);
        }

        // We increase the expected delivery quality level every time a package isn't delivered.
        // This will trigger a cancellation after a while.
        UpdateDeliveryQualityExpectation();

        // Update the loyality level based on the decay value.
        // By updating this, we slowly decrease the interest of the customer making it more likely
        // that the customer will cancel the subscription.
        UpdateLoyalityLevel();

        Sender.Tell(OK.Instance);
    }

    private async Task OnShipmentDelivered(Guid shippingOrderId)
    {
        Context.GetLogger().Info("Yay, got another pack of coffee");

        using var activity = Activities.ShipmentDelivered();

        var shippingOrder = await _shippingInformation.GetShippingOrderAsync(shippingOrderId);
        var deliveredProduct = shippingOrder.OrderItems.First();

        // Determine whether this product was actually the one we want.
        // Currently, we simply assign a random rating as our best shot. 
        // May want to choose a better way to simulate this.

        var rating = _random.Next(1, 6);
        await _ratings.RateProductAsync(_customerId, deliveredProduct.ProductId, rating);

        _previousRatings.Add(rating);

        var productQuality = _previousRatings.Average() / 5;

        // When the product no longer meets the expectation we'll do a loyality check
        // to determine whether the customer still wants to subscribe to our services.
        if (productQuality < _customerProfile.ProductQuality && IsNoLongerLoyal())
        {
            Context.GetLogger()
                .Warning("Customer doesn't like the quality of the product and no longer wishes to subscribe.");
            await _subscriptions.CancelSubscriptionAsync(_customerId);
        }

        // Finally, when we've rated the product we determine if the customer still is interested in general
        // by performing another loyalty check.
        if (IsNoLongerLoyal())
        {
            Context.GetLogger().Warning("Customer is no longer interested.");
            await _subscriptions.CancelSubscriptionAsync(_customerId);
        }

        // Update the loyality level based on the decay value.
        // By updating this, we slowly decrease the interest of the customer making it more likely
        // that the customer will cancel the subscription.
        UpdateLoyalityLevel();

        Sender.Tell(OK.Instance);
    }

    private bool IsNoLongerLoyal()
    {
        return _random.Next() < 1 - _currentLoyaltyLevel;
    }

    private void UpdateDeliveryQualityExpectation()
    {
        _currentDeliveryQualityLevel *= (1 + _customerProfile.DeliveryQualityGrowth);
    }

    private void UpdateLoyalityLevel()
    {
        _currentLoyaltyLevel *= (1 - _customerProfile.LoyalityDecay);
    }
}