using Jasper.Attributes;

namespace TastyBeans.Profile.Domain.Aggregates.CustomerAggregate.Events;

public record CustomerRegistered(
    Guid CustomerId, 
    string FirstName,
    string LastName,
    Address ShippingAddress,
    Address InvoiceAddress,
    string EmailAddress);