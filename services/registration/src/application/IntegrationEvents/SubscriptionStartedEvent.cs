using TastyBeans.Registration.Domain.Subscriptions;

namespace TastyBeans.Registration.Application.IntegrationEvents;

public record SubscriptionStartedEvent(
    Guid CustomerId, 
    DateTime StartDate, 
    ShippingFrequency ShippingFrequency,
    SubscriptionKind Kind);