namespace TastyBeans.Registration.Domain.Subscriptions;

public record RegisterSubscriptionRequest(Guid CustomerId, ShippingFrequency Frequency, SubscriptionKind Kind);