namespace TastyBeans.Ratings.Domain.Aggregates.ProductAggregate.Commands;

public record RegisterProductCommand(Guid Id, string Name);