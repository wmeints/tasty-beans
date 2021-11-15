using System.Diagnostics.CodeAnalysis;

namespace RecommendCoffee.Catalog.Domain.Projections.ProductInfoProjection;

public class ProductInformation
{
    private ProductInformation(Guid id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
    }

    public ProductInformation(Guid id, string name, string description, IEnumerable<ProductVariantInformation> productVariants)
    {
        Id = id;
        Name = name;
        Description = description;
        ProductVariants = productVariants;
    }

    public Guid Id { get; private set; }

    public string Name { get; private set; }

    public string Description { get; private set; }

    public IEnumerable<ProductVariantInformation>? ProductVariants { get; private set; }
}
