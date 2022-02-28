using RecommendCoffee.Subscriptions.Domain.Common;

namespace RecommendCoffee.Subscriptions.Domain.Aggregates.SubscriptionAggregate.Events;

public record SubscriptionCancelledEvent(Guid CustomerId, DateTime EndDate): IDomainEvent;