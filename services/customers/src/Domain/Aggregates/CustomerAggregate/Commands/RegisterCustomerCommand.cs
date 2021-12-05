namespace RecommendCoffee.Customers.Domain.Aggregates.CustomerAggregate.Commands;

public record RegisterCustomerCommand(string FirstName, string LastName, Address InvoiceAddress, Address ShippingAddress);
