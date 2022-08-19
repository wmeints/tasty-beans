using TastyBeans.Registration.Api.Application.Commands;
using TastyBeans.Registration.Api.Models;

namespace TastyBeans.Registration.Api.Infrastructure.Clients.Profile;

public record RegisterCustomerRequest(
    Guid CustomerId,
    string FirstName,
    string LastName,
    Address ShippingAddress,
    Address InvoiceAddress,
    string EmailAddress);
