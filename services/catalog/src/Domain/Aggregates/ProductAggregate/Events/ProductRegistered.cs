using System.Diagnostics.CodeAnalysis;

namespace RecommendCoffee.Catalog.Domain.Aggregates.ProductAggregate.Events;

public record ProductRegistered: Event
{
    private ProductRegistered()
    {

    }

    public ProductRegistered(Guid productId, string name, string description, IEnumerable<ProductVariant> variants)
    {
        EventId = Guid.NewGuid();
        ProductId = productId;
        Name = name;
        Description = description;
        Variants = variants;
    }

    [NotNull]
    public string? Name { get; private set; }

    [NotNull]
    public string? Description { get; private set; }

    [NotNull]
    public IEnumerable<ProductVariant>? Variants { get; private set; }

    public Guid ProductId { get; private set; }
}