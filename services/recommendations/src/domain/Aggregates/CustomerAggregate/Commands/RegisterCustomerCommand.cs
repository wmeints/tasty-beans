namespace RecommendCoffee.Recommendations.Domain.Aggregates.CustomerAggregate.Commands;

public record RegisterCustomerCommand(Guid Id, string FirstName, string LastName, string EmailAddress, Address ShippingAddress);