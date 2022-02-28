using Domain.Aggregates.ProductAggregate;

namespace RecommendCoffee.Catalog.Domain.Aggregates.ProductAggregate.Commands;

public record RegisterProductCommand(string Name, string Description, IEnumerable<ProductVariant> Variants);