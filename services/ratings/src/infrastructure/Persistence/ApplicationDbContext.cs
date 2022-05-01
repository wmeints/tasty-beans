using Microsoft.EntityFrameworkCore;
using TastyBeans.Ratings.Domain.Aggregates.CustomerAggregate;
using TastyBeans.Ratings.Domain.Aggregates.ProductAggregate;
using TastyBeans.Ratings.Domain.Aggregates.RatingAggregate;

namespace TastyBeans.Ratings.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Rating> Ratings => Set<Rating>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ProductEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new CustomerEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new RatingEntityTypeConfiguration());
    }
}