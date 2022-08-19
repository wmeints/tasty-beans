using TastyBeans.Profile.Domain.Aggregates.CustomerAggregate;

namespace TastyBeans.Profile.Api.Application.Commands;

public record RegisterCustomerCommand(
    Guid CustomerId,
    string FirstName,
    string LastName,
    Address ShippingAddress,
    Address InvoiceAddress, 
    string EmailAddress,
    DateTime SubscriptionStartDate);