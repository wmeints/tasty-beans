using TastyBeans.Shared.Domain;

namespace TastyBeans.Subscriptions.Domain.Aggregates.SubscriptionAggregate.Events;

[Topic("subscriptions.subscription.started.v1")]
public record SubscriptionStartedEvent(
    Guid CustomerId, 
    DateTime StartDate, 
    ShippingFrequency ShippingFrequency,
    SubscriptionKind Kind): IDomainEvent;