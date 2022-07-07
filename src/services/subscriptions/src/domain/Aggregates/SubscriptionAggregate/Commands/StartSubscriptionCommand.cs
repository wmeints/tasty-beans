namespace TastyBeans.Subscriptions.Domain.Aggregates.SubscriptionAggregate.Commands;

public record StartSubscriptionCommand(Guid CustomerId, SubscriptionKind Kind, ShippingFrequency ShippingFrequency);
