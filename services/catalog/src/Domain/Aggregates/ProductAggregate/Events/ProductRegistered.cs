namespace RecommendCoffee.Catalog.Domain.Aggregates.ProductAggregate.Events;

[Topic("catalog.fct.product-registered")]
public record ProductRegistered(Guid ProductId, string Name, string Description, IEnumerable<ProductVariant> Variants): Event;