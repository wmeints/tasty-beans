using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TastyBeans.Shipping.Domain.Aggregates.CustomerAggregate;
using TastyBeans.Shipping.Domain.Aggregates.ProductAggregate;
using TastyBeans.Shipping.Domain.Aggregates.ShippingOrderAggregate;

namespace TastyBeans.Shipping.Infrastructure.Persistence;

public class ShippingOrderEntityTypeConfiguration: IEntityTypeConfiguration<ShippingOrder>
{
    public void Configure(EntityTypeBuilder<ShippingOrder> builder)
    {
        builder.Property<byte[]>("Version").IsRowVersion();
        
        var orderItemsNavigation = builder.OwnsMany(x => x.OrderItems);
        orderItemsNavigation.HasOne<Product>().WithMany().HasForeignKey(x => x.ProductId);

        builder.HasOne<Customer>().WithMany().HasForeignKey("CustomerId");
    }
}