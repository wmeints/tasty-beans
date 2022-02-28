namespace RecommendCoffee.Catalog.Domain.Aggregates.ProductAggregate.Commands;

public record DiscontinueProductCommand(Guid ProductId);