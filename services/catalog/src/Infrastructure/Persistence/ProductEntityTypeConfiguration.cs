using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecommendCoffee.Catalog.Domain.Aggregates.ProductAggregate;

namespace Infrastructure.Persistence;

public class ProductEntityTypeConfiguration: IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Property(x => x.Name).IsRequired().HasMaxLength(500);
        builder.Property(x => x.Description).IsRequired();
        
        var variantsNavigation = builder.OwnsMany(x => x.Variants);

        variantsNavigation.Property(x => x.Weight).IsRequired();
        variantsNavigation.Property(x => x.UnitPrice).HasPrecision(5, 2).IsRequired();

        builder.HasIndex(x => x.Name);
    }
}