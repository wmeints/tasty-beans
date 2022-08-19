using Jasper.Attributes;

namespace TastyBeans.Profile.Api.Application.IntegrationEvents;

[MessageIdentity("profiles.customer.subscription-started.v1")]
public record SubscriptionStartedIntegrationEvent(Guid CustomerId, DateTime StartDate);
