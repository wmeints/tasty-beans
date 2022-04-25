using Domain.Aggregates.ProductAggregate;

namespace RecommendCoffee.Catalog.Domain.Aggregates.ProductAggregate.Commands;

public record UpdateProductCommand(
    Guid ProductId, 
    string Name, 
    string Description,
    IEnumerable<ProductVariant> Variants);
