using Microsoft.EntityFrameworkCore;
using RecommendCoffee.Subscriptions.Domain.Aggregates.SubscriptionAggregate;

namespace RecommendCoffee.Subscriptions.Infrastructure.Persistence;

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