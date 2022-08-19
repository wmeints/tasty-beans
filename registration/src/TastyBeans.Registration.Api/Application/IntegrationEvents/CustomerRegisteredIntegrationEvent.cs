using Jasper.Attributes;
using Jasper.Persistence.Sagas;
using TastyBeans.Registration.Api.Models;

namespace TastyBeans.Registration.Api.Application.IntegrationEvents;

[MessageIdentity("profiles.customer.registered.v1")]
public record CustomerRegisteredIntegrationEvent(Guid CustomerId, string FirstName, string LastName,
    Address ShippingAddress, Address InvoiceAddress, string EmailAddress)
{
    [SagaIdentity]
    public Guid CustomerId { get; init; } = CustomerId;
}
