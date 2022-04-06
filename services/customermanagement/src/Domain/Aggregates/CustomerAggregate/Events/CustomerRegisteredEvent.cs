using RecommendCoffee.CustomerManagement.Domain.Common;

namespace RecommendCoffee.CustomerManagement.Domain.Aggregates.CustomerAggregate.Events;

[Topic("customermanagement.customer.registered.v1")]
public record CustomerRegisteredEvent(
    Guid CustomerId, 
    string FirstName, 
    string LastName, 
    Address InvoiceAddress, 
    Address ShippingAddress, 
    string EmailAddress, 
    string TelephoneNumber
): IDomainEvent;
