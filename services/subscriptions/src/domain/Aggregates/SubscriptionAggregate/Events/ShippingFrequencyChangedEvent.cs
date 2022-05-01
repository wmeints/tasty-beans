using TastyBeans.Shared.Domain;

namespace TastyBeans.Subscriptions.Domain.Aggregates.SubscriptionAggregate.Events;

[Topic("subscriptions.subscription.shippinfrequencychanged.v1")]
public record ShippingFrequencyChangedEvent(Guid SubscriptionId, ShippingFrequency ShippingFrequency): IDomainEvent;
