using Microsoft.EntityFrameworkCore;
using TastyBeans.Subscriptions.Domain.Aggregates.SubscriptionAggregate;

namespace TastyBeans.Subscriptions.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Subscription> Subscriptions => Set<Subscription>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new SubscriptionEntityTypeConfiguration());
    }
}