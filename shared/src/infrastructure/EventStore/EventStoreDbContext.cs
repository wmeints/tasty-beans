using Microsoft.EntityFrameworkCore;

namespace TastyBeans.Shared.Infrastructure.EventStore;

public class EventStoreDbContext : DbContext
{
    public DbSet<DomainEventRecord> DomainEvents => Set<DomainEventRecord>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DomainEventRecord>().HasKey(x => x.Id).IsClustered();
        
        // Make sure we have a unique combination of aggregate and sequence number as a concurrency measure.
        modelBuilder.Entity<DomainEventRecord>()
            .HasIndex(x => new { x.AggregateId, x.SequenceNumber }).IsUnique();

        // The payload type can be limited in size, we want speed :-)
        modelBuilder.Entity<DomainEventRecord>().Property(x => x.PayloadType).HasMaxLength(255);
    }
}