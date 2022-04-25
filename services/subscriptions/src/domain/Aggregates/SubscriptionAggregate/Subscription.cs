using NodaTime;
using NodaTime.Extensions;
using RecommendCoffee.Subscriptions.Domain.Aggregates.SubscriptionAggregate.Commands;
using RecommendCoffee.Subscriptions.Domain.Aggregates.SubscriptionAggregate.Events;
using RecommendCoffee.Subscriptions.Domain.Aggregates.SubscriptionAggregate.Validators;
using RecommendCoffee.Subscriptions.Domain.Services.Recommendations;
using RecommendCoffee.Subscriptions.Domain.Services.Shipping;
using RecommendCoffee.Subscriptions.Domain.Services.Shipping.Commands;

namespace RecommendCoffee.Subscriptions.Domain.Aggregates.SubscriptionAggregate;

public class Subscription
{
    private Subscription()
    {
    }

    public Subscription(
        Guid id,
        DateTime startDate,
        ShippingFrequency shippingFrequency,
        SubscriptionKind kind)
    {
        Id = id;
        StartDate = startDate;
        ShippingFrequency = shippingFrequency;
        Kind = kind;
    }

    public Guid Id { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }
    public ShippingFrequency ShippingFrequency { get; private set; }
    public SubscriptionKind Kind { get; private set; }

    public bool IsActive => EndDate == null || EndDate < DateTime.UtcNow; 
    
    public CancelSubscriptionCommandReply Cancel(CancelSubscriptionCommand command)
    {
        using var activity = Activities.Unsubscribe(command.CustomerId);
        
        var validator = new CancelSubscriptionCommandValidator();
        var validationResult = validator.Validate(command);
        
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .Select(x => new ValidationError(x.PropertyName, x.ErrorMessage))
                .ToList();

            return new CancelSubscriptionCommandReply(
                errors, Enumerable.Empty<IDomainEvent>());
        }

        // Subscriptions can only be cancelled at the end of the month.
        // If you payed for the current month you'll receive a final shipment before the subscription is cancelled.
        var currentDate = DateTime.UtcNow.Date.ToLocalDateTime().Date;
        var endDate = DateAdjusters.EndOfMonth(currentDate).ToDateTimeUnspecified();

        EndDate = endDate;
        
        var evt = new SubscriptionCancelledEvent(Id, endDate);

        return new CancelSubscriptionCommandReply(
            Enumerable.Empty<ValidationError>(), 
            new IDomainEvent[] { evt });
    }

    public ChangeShippingFrequencyCommandReply ChangeShippingFrequency(ChangeShippingFrequencyCommand command)
    {
        using var activity = Activities.ChangeShippingFrequency(command.CustomerId);
        
        if (command.ShippingFrequency != ShippingFrequency)
        {
            ShippingFrequency = command.ShippingFrequency;
            var evt = new ShippingFrequencyChangedEvent(Id, ShippingFrequency);

            return new ChangeShippingFrequencyCommandReply(
                Enumerable.Empty<ValidationError>(),
                new IDomainEvent[] { evt });
        }

        return new ChangeShippingFrequencyCommandReply(
            Enumerable.Empty<ValidationError>(),
            Enumerable.Empty<IDomainEvent>());
    }

    public async Task<CreateShipmentCommandResponse> CreateShipment(
        CreateShipmentCommand cmd, IRecommendations recommendations, IShipping shipping)
    {
        var recommendation = await recommendations.GetRecommendProductAsync(Id);
        await shipping.CreateShippingOrderAsync(new CreateShippingOrderCommand(
            Id, recommendation));

        var shipmentCreatedEvent = new ShipmentCreatedEvent(Id, recommendation);

        Metrics.ShipmentsCreated.Add(1);
        
        return new CreateShipmentCommandResponse(
            Enumerable.Empty<ValidationError>(), 
            new[] { shipmentCreatedEvent });
    }

    public StartSubscriptionCommandReply Resubscribe(StartSubscriptionCommand command)
    {
        using var activity = Activities.Subscribe(command.CustomerId);
        
        Kind = command.Kind;
        ShippingFrequency = command.ShippingFrequency;
        StartDate = DateTime.UtcNow;
        EndDate = null;
        
        var evt = new SubscriptionStartedEvent(
            Id, StartDate, ShippingFrequency, Kind);

        return new StartSubscriptionCommandReply(
            this,
            Enumerable.Empty<ValidationError>(),
            new IDomainEvent[] { evt });
    }
    
    public static StartSubscriptionCommandReply Start(StartSubscriptionCommand command)
    {
        using var activity = Activities.Subscribe(command.CustomerId);
        
        var validator = new StartSubscriptionCommandValidator();
        var validationResult = validator.Validate(command);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .Select(x => new ValidationError(x.PropertyName, x.ErrorMessage))
                .ToList();

            return new StartSubscriptionCommandReply(
                null, errors, Enumerable.Empty<IDomainEvent>());
        }

        var instance = new Subscription(
            command.CustomerId,
            DateTime.UtcNow,
            command.ShippingFrequency,
            command.Kind);

        var evt = new SubscriptionStartedEvent(
            instance.Id, instance.StartDate,
            instance.ShippingFrequency, instance.Kind);

        return new StartSubscriptionCommandReply(
            instance,
            Enumerable.Empty<ValidationError>(),
            new IDomainEvent[] { evt });
    }
}