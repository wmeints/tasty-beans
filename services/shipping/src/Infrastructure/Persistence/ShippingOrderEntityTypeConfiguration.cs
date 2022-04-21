using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecommendCoffee.Shipping.Domain.Aggregates.CustomerAggregate;
using RecommendCoffee.Shipping.Domain.Aggregates.ProductAggregate;
using RecommendCoffee.Shipping.Domain.Aggregates.ShippingOrderAggregate;

namespace RecommendCoffee.Shipping.Infrastructure.Persistence;

public class ShippingOrderEntityTypeConfiguration: IEntityTypeConfiguration<ShippingOrder>
{
    public void Configure(EntityTypeBuilder<ShippingOrder> builder)
    {
        builder.Property<byte[]>("Version").IsRowVersion();
        
        var orderItemsNavigation = builder.OwnsMany(x => x.OrderItems);
        
        orderItemsNavigation.Property(x => x.Description).IsRequired().HasMaxLength(250);
        orderItemsNavigation.HasOne<Product>().WithMany().HasForeignKey(x => x.ProductId);

        builder.HasOne<Customer>().WithMany().HasForeignKey("CustomerId");
    }
}