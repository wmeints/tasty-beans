namespace RecommendCoffee.CustomerManagement.Domain.Aggregates.CustomerAggregate.Commands;

public record RegisterCustomerCommand(Guid CustomerId, string FirstName, string LastName, Address InvoiceAddress, Address ShippingAddress, string EmailAddress, string TelephoneNumber);
