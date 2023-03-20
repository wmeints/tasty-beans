using Microsoft.EntityFrameworkCore;
using RecommendCoffee.Payments.Domain.Aggregates.PaymentMethodAggregate;

namespace RecommendCoffee.Payments.Infrastructure.Persistence;

public class ApplicationDbContext: DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<PaymentMethod> PaymentMethods => Set<PaymentMethod>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new PaymentMethodEntityTypeConfiguration());
    }
}