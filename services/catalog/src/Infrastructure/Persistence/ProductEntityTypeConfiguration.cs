using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecommendCoffee.Catalog.Domain.Aggregates.ProductAggregate;

namespace RecommendCoffee.Catalog.Infrastructure.Persistence;

public class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Property<byte[]>("Version").IsRowVersion();
        
        builder.Property(x => x.Name).IsRequired().HasMaxLength(500);
        builder.Property(x => x.Description).IsRequired();

        builder
            .Property(x => x.FlavorNotes)
            .HasConversion(
                input => string.Join(';', input != null ? input.OrderBy(x => x) : ""),
                output => output.Split(';', StringSplitOptions.TrimEntries).OrderBy(x => x).ToList(),
                new ValueComparer<List<string>>(
                    (c1, c2) => c1 != null && c2 != null && c1.SequenceEqual(c2),
                    c=> c.Aggregate(0, (a,v) => HashCode.Combine(a,v.GetHashCode())),
                    c => c.ToList()
                )
            );

        builder.Property(x => x.Taste).HasMaxLength(100);

        var variantsNavigation = builder.OwnsMany(x => x.Variants);

        variantsNavigation.Property(x => x.Weight).IsRequired();
        variantsNavigation.Property(x => x.UnitPrice).HasPrecision(5, 2).IsRequired();

        builder.HasIndex(x => x.Name);
    }
}