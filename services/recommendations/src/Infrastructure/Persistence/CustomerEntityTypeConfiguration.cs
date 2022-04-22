using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecommendCoffee.Recommendations.Domain.Aggregates.CustomerAggregate;

namespace RecommendCoffee.Recommendations.Infrastructure.Persistence;

public class CustomerEntityTypeConfiguration: IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.Property<byte[]>("Version").IsRowVersion();
        builder.Property(x => x.FirstName).IsRequired().HasMaxLength(150);
        builder.Property(x => x.LastName).IsRequired().HasMaxLength(150);
        builder.Property(x => x.EmailAddress).HasMaxLength(500).IsRequired();
        
        var shippingAddressNavigation = builder.OwnsOne(x => x.ShippingAddress);
        
        shippingAddressNavigation.Property(x=>x.Street).HasMaxLength(100).IsRequired();
        shippingAddressNavigation.Property(x=>x.HouseNumber).HasMaxLength(20).IsRequired();
        shippingAddressNavigation.Property(x=>x.PostalCode).HasMaxLength(20).IsRequired();
        shippingAddressNavigation.Property(x=>x.City).HasMaxLength(100).IsRequired();
        shippingAddressNavigation.Property(x=>x.CountryCode).HasMaxLength(10).IsRequired();
    }
}