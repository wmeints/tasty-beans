namespace RecommendCoffee.Catalog.Domain.Aggregates.ProductAggregate.Events;

[Topic("catalog.product.updated.v1")]
public record ProductUpdatedEvent(
    Guid ProductId, 
    string Name, 
    string Description, 
    IEnumerable<ProductVariant> Variants): IDomainEvent;