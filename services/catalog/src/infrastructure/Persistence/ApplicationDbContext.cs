using Microsoft.EntityFrameworkCore;
using RecommendCoffee.Catalog.Domain.Aggregates.ProductAggregate;

namespace RecommendCoffee.Catalog.Infrastructure.Persistence;

public class ApplicationDbContext: DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new ProductEntityTypeConfiguration());
    }
}