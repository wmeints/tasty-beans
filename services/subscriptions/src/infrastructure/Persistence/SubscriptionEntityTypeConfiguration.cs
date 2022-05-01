using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TastyBeans.Subscriptions.Domain.Aggregates.SubscriptionAggregate;

namespace TastyBeans.Subscriptions.Infrastructure.Persistence;

public class SubscriptionEntityTypeConfiguration: IEntityTypeConfiguration<Subscription>
{
    public void Configure(EntityTypeBuilder<Subscription> builder)
    {
        builder.Property(x => x.Kind).IsRequired();
        builder.Property(x => x.ShippingFrequency).IsRequired();
        builder.Property<byte[]>("Version").IsRowVersion();
        
        builder.Ignore(x => x.IsActive);
    }
}