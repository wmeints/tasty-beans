using Jasper.Attributes;

namespace TastyBeans.Profile.Api.Application.IntegrationEvents;

[MessageIdentity("profile.customer.subscription-ended.v1")]
public record SubscriptionEndedIntegrationEvent(Guid CustomerId)
{
    
}