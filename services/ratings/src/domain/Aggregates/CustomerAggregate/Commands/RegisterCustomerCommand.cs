namespace TastyBeans.Ratings.Domain.Aggregates.CustomerAggregate.Commands;

public record RegisterCustomerCommand(Guid Id, string FirstName, string LastName);