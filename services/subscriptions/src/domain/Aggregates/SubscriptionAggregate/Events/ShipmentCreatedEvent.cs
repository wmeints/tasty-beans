namespace RecommendCoffee.Subscriptions.Domain.Aggregates.SubscriptionAggregate.Events;

[Topic("subscriptions.subscription.shipmentcreated.v1")]
public record ShipmentCreatedEvent(Guid CustomerId, Guid ProductId) : IDomainEvent;