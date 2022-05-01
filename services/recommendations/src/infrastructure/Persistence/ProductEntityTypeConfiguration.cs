using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TastyBeans.Recommendations.Domain.Aggregates.ProductAggregate;

namespace TastyBeans.Recommendations.Infrastructure.Persistence;

public class ProductEntityTypeConfiguration: IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Property<byte[]>("Version").IsRowVersion();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(500);
    }
}