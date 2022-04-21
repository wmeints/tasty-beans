using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecommendCoffee.Shipping.Domain.Aggregates.ProductAggregate;

namespace RecommendCoffee.Shipping.Infrastructure.Persistence;

public class ProductEntityTypeConfiguration: IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Property<byte[]>("Version").IsRowVersion();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(500);
    }
}