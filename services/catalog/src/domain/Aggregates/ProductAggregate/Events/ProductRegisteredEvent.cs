namespace RecommendCoffee.Catalog.Domain.Aggregates.ProductAggregate.Events;

[Topic("catalog.product.registered.v1")]
public record ProductRegisteredEvent(
    Guid ProductId,
    string Name,
    string Description,
    IEnumerable<ProductVariant> Variants): IDomainEvent;