using RecommendCoffee.Registration.Domain.Subscriptions;

namespace RecommendCoffee.Registration.Application.IntegrationEvents;

public record SubscriptionStartedEvent(
    Guid CustomerId, 
    DateTime StartDate, 
    ShippingFrequency ShippingFrequency,
    SubscriptionKind Kind);