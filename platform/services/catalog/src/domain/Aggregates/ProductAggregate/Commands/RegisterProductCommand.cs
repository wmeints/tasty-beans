namespace TastyBeans.Catalog.Domain.Aggregates.ProductAggregate.Commands;

public record RegisterProductCommand(string Name, string Description, IEnumerable<ProductVariant> Variants);