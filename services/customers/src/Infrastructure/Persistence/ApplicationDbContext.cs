using Microsoft.EntityFrameworkCore;
using RecommendCoffee.Customers.Domain.Projections.CustomerInfoProjection;

namespace RecommendCoffee.Customers.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }

    public DbSet<EventData<Guid>> CustomerEvents => Set<EventData<Guid>>("CustomerEvents");

    public DbSet<CustomerInformation> Customers => Set<CustomerInformation>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<CustomerInformation>().Property(x => x.FirstName).HasMaxLength(100);
        modelBuilder.Entity<CustomerInformation>().Property(x => x.LastName).HasMaxLength(100);

        var invoiceAddressModel = modelBuilder
            .Entity<CustomerInformation>()
            .OwnsOne(x => x.InvoiceAddress);

        var shippingAddressModel = modelBuilder
            .Entity<CustomerInformation>()
            .OwnsOne(x => x.ShippingAddress);
        
        invoiceAddressModel.Property(x => x.StreetName).HasMaxLength(500);
        invoiceAddressModel.Property(x => x.City).HasMaxLength(100);
        invoiceAddressModel.Property(x => x.HouseNumber).HasMaxLength(20);
        invoiceAddressModel.Property(x => x.ZipCode).HasMaxLength(20);
        
        shippingAddressModel.Property(x => x.StreetName).HasMaxLength(500);
        shippingAddressModel.Property(x => x.City).HasMaxLength(100);
        shippingAddressModel.Property(x => x.HouseNumber).HasMaxLength(20);
        shippingAddressModel.Property(x => x.ZipCode).HasMaxLength(20);
        
        modelBuilder.SharedTypeEntity<EventData<Guid>>("CustomerEvents", options =>
        {
            options.Property<int>("Sequence").UseIdentityColumn();
            options.HasKey(x => x.EventId);
            options.HasIndex(x => x.AggregateId);
        });
    }
}
