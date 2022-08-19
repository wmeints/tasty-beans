using Jasper.Attributes;

namespace TastyBeans.Profile.Api.Application.IntegrationEvents;

[MessageIdentity("profile.customer.subscription-cancelled.v1")]
public record SubscriptionCancelledIntegrationEvent(Guid CustomerId, DateTime EndDate);