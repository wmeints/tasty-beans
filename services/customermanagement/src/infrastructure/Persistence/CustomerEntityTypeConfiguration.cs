using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecommendCoffee.CustomerManagement.Domain.Aggregates.CustomerAggregate;

namespace RecommendCoffee.CustomerManagement.Infrastructure.Persistence;

public class CustomerEntityTypeConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.Property<byte[]>("Version").IsRowVersion();
        builder.Property(x => x.FirstName).HasMaxLength(100).IsRequired();
        builder.Property(x => x.LastName).HasMaxLength(100).IsRequired();
        builder.Property(x => x.TelephoneNumber).HasMaxLength(13).IsRequired();
        builder.Property(x => x.EmailAddress).HasMaxLength(500).IsRequired();

        var invoiceAddress = builder.OwnsOne(x=>x.InvoiceAddress);
        var shippingAddress = builder.OwnsOne(x=>x.ShippingAddress);

        invoiceAddress.Property(x=>x.Street).HasMaxLength(100).IsRequired();
        invoiceAddress.Property(x=>x.HouseNumber).HasMaxLength(20).IsRequired();
        invoiceAddress.Property(x=>x.PostalCode).HasMaxLength(20).IsRequired();
        invoiceAddress.Property(x=>x.City).HasMaxLength(100).IsRequired();
        invoiceAddress.Property(x=>x.CountryCode).HasMaxLength(10).IsRequired();

        shippingAddress.Property(x=>x.Street).HasMaxLength(100).IsRequired();
        shippingAddress.Property(x=>x.HouseNumber).HasMaxLength(20).IsRequired();
        shippingAddress.Property(x=>x.PostalCode).HasMaxLength(20).IsRequired();
        shippingAddress.Property(x=>x.City).HasMaxLength(100).IsRequired();
        shippingAddress.Property(x=>x.CountryCode).HasMaxLength(10).IsRequired();
    }
}