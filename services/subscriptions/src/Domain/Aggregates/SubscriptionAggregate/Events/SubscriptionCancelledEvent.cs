using RecommendCoffee.Subscriptions.Domain.Common;

namespace RecommendCoffee.Subscriptions.Domain.Aggregates.SubscriptionAggregate.Events;

[Topic("subscriptions.subscription.cancelled.v1")]
public record SubscriptionCancelledEvent(Guid CustomerId, DateTime EndDate): IDomainEvent;