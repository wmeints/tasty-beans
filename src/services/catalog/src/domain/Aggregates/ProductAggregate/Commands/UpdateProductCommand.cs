namespace TastyBeans.Catalog.Domain.Aggregates.ProductAggregate.Commands;

public record UpdateProductCommand(
    Guid ProductId, 
    string Name, 
    string Description,
    IEnumerable<ProductVariant> Variants);
