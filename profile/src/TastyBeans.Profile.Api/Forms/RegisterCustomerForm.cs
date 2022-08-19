using TastyBeans.Profile.Domain.Aggregates.CustomerAggregate;

namespace TastyBeans.Profile.Api.Forms;

public record RegisterCustomerForm(
    Guid CustomerId,
    string FirstName,
    string LastName,
    Address ShippingAddress,
    Address InvoiceAddress,
    string EmailAddress);