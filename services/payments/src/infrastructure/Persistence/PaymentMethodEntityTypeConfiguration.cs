using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecommendCoffee.Payments.Domain.Aggregates.PaymentMethodAggregate;

namespace RecommendCoffee.Payments.Infrastructure.Persistence;

public class PaymentMethodEntityTypeConfiguration: IEntityTypeConfiguration<PaymentMethod>
{
    public void Configure(EntityTypeBuilder<PaymentMethod> builder)
    {
        builder.Property<byte[]>("Version").IsRowVersion();
        builder.Property(x => x.CardNumber).IsRequired().HasMaxLength(16);
        builder.Property(x => x.ExpirationDate).IsRequired().HasMaxLength(5);
        builder.Property(x => x.SecurityCode).IsRequired().HasMaxLength(3);
        builder.Property(x => x.CardHolderName).IsRequired().HasMaxLength(150);
    }
}