namespace RecommendCoffee.Catalog.Domain.Aggregates.ProductAggregate.Events;

public record ProductRegistered(Guid ProductId, string Name, string Description, IEnumerable<ProductVariant> Variants): Event;