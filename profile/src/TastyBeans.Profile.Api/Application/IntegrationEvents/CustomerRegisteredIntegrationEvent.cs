using Jasper.Attributes;
using TastyBeans.Profile.Domain.Aggregates.CustomerAggregate;

namespace TastyBeans.Profile.Api.Application.IntegrationEvents;

[MessageIdentity("profiles.customer.registered.v1")]
public record CustomerRegisteredIntegrationEvent(Guid CustomerId, string FirstName, string LastName,
    Address ShippingAddress, Address InvoiceAddress, string EmailAddress);
