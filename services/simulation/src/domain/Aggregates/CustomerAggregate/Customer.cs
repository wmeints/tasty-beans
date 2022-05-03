using Akka.Actor;
using Akka.Event;
using TastyBeans.Simulation.Domain.Aggregates.CustomerAggregate.Commands;
using TastyBeans.Simulation.Domain.Services.ShippingInformation;

namespace TastyBeans.Simulation.Domain.Aggregates.CustomerAggregate;

public class Customer: ReceiveActor
{
    private readonly Guid _customerId;
    private readonly CustomerProfile _customerProfile;
    private readonly IShippingInformation _shippingInformation;

    private double _currentLoyaltyLevel;
    private double _currentDeliveryQualityLevel;
    private double _currentProductQualityLevel;

    public Customer(Guid customerId, CustomerProfile customerProfile, IShippingInformation shippingInformation)
    {
        _customerId = customerId;
        _customerProfile = customerProfile;
        _shippingInformation = shippingInformation;

        _currentLoyaltyLevel = customerProfile.Loyality;
        _currentDeliveryQualityLevel = customerProfile.DeliveryQuality;
        _currentProductQualityLevel = 5;
        
        Receive<ShipmentDelivered>(msg => OnShipmentDelivered(msg.ShippingOrderId));
        Receive<ShipmentLost>(msg => OnShipmentLost(msg.ShippingOrderId));
        
        // The customer gets all kinds of messages related to deliveries.
        // We'll handle them all with a generic handler if not handled above.
        // We've done this to make sure our logs don't overflow with unwanted errors.
        
        ReceiveAny(msg => { });
    }

    private void OnShipmentLost(Guid msgShippingOrderId)
    {
        //TODO: Determine whether to cancel the subscription based on the delivery quality level.
        //TODO: Update delivery quality level based on decay function. 
    }

    private void OnShipmentDelivered(Guid shippingOrderId)
    {
        //TODO: Retrieve the shipping order to determine the delivered product.
        //TODO: Retrieve whether this product was actually the one we wanted.
        //TODO: Rate the current shipment using a random rating.
        //TODO: Update running average of product ratings.
        //TODO: Decide whether to cancel the subscription based on product quality.
        //TODO: Decide whether to cancel the subscription based on loyalty.
    }

    public static Props Props(Guid customerId, CustomerProfile customerProfile, IShippingInformation shippingInformation)
    {
        return new Props(
            type: typeof(Customer),
            supervisorStrategy: Akka.Actor.SupervisorStrategy.DefaultStrategy, 
            args: new object[] { customerId, customerProfile, shippingInformation });
    }
}