using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecommendCoffee.Subscriptions.Domain.Aggregates.SubscriptionAggregate;

namespace RecommendCoffee.Subscriptions.Infrastructure.Persistence;

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