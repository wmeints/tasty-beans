using Microsoft.EntityFrameworkCore;
using RecommendCoffee.Catalog.Domain.Projections.ProductInfoProjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendCoffee.Catalog.Infrastructure.Persistence
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {

        }

        public DbSet<ProductInformation> Products => Set<ProductInformation>();
        public DbSet<EventData<Guid>> ProductEvents => Set<EventData<Guid>>("ProductEvents");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.SharedTypeEntity<EventData<Guid>>("ProductEvents", options =>
            {
                options.Property<int>("Sequence").UseIdentityColumn();
                options.HasKey(x => x.EventId);
                options.HasIndex(x => x.AggregateId);
            });

            var productInformationEntity = modelBuilder.Entity<ProductInformation>();

            productInformationEntity.Property(x => x.Name).HasMaxLength(500).IsRequired();
            productInformationEntity.Property(x => x.Description).IsRequired();

            productInformationEntity.OwnsMany(x => x.ProductVariants).Property(x => x.UnitPrice).HasPrecision(5,2);
        }
    }
}
