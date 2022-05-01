using Microsoft.EntityFrameworkCore;
using TastyBeans.CustomerManagement.Domain.Aggregates.CustomerAggregate;

namespace TastyBeans.CustomerManagement.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {

    }

    public DbSet<Customer> Customers => Set<Customer>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new CustomerEntityTypeConfiguration());
    }
}
