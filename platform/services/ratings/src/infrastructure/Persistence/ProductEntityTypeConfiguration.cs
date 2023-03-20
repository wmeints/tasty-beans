using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TastyBeans.Ratings.Domain.Aggregates.ProductAggregate;

namespace TastyBeans.Ratings.Infrastructure.Persistence;

public class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Property<byte[]>("Version").IsRowVersion();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(500);
    }
}
