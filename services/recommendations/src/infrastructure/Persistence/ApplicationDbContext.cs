using Microsoft.EntityFrameworkCore;
using RecommendCoffee.Recommendations.Domain.Aggregates.CustomerAggregate;
using RecommendCoffee.Recommendations.Domain.Aggregates.ProductAggregate;

namespace RecommendCoffee.Recommendations.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Customer> Customers => Set<Customer>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new ProductEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new CustomerEntityTypeConfiguration());
    }
}