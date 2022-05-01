using Microsoft.EntityFrameworkCore;
using TastyBeans.Shipping.Domain.Aggregates.CustomerAggregate;
using TastyBeans.Shipping.Domain.Aggregates.ProductAggregate;
using TastyBeans.Shipping.Domain.Aggregates.ShippingOrderAggregate;

namespace TastyBeans.Shipping.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<ShippingOrder> ShippingOrders => Set<ShippingOrder>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new ProductEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new CustomerEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ShippingOrderEntityTypeConfiguration());
    }
}