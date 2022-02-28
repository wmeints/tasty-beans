using RecommendCoffee.Subscriptions.Domain.Common;

namespace RecommendCoffee.Subscriptions.Domain.Aggregates.SubscriptionAggregate.Events;

public record ShippingFrequencyChangedEvent(Guid SubscriptionId, ShippingFrequency ShippingFrequency): IDomainEvent;
