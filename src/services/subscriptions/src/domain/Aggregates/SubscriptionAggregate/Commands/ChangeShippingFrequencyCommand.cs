namespace TastyBeans.Subscriptions.Domain.Aggregates.SubscriptionAggregate.Commands;

public record ChangeShippingFrequencyCommand(Guid CustomerId, ShippingFrequency ShippingFrequency);
