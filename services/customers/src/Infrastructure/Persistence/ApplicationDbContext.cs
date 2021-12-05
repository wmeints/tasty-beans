using Microsoft.EntityFrameworkCore;

namespace RecommendCoffee.Customers.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }

    public DbSet<EventData<Guid>> CustomerEvents => Set<EventData<Guid>>("CustomerEvents");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.SharedTypeEntity<EventData<Guid>>("CustomerEvents", options =>
        {
            options.Property<int>("Sequence").UseIdentityColumn();
            options.HasKey(x => x.EventId);
            options.HasIndex(x => x.AggregateId);
        });
    }
}
