using System.Diagnostics.CodeAnalysis;
using TastyBeans.Shared.Domain;

namespace RecommendCoffee.Payments.Domain.Aggregates.PaymentMethodAggregate.Commands;

public record RegisterPaymentMethodReply(
    PaymentMethod? PaymentMethod, 
    IEnumerable<ValidationError> Errors,
    IEnumerable<IDomainEvent> Events)
{
    [MemberNotNullWhen(true,nameof(PaymentMethod))]
    public bool IsValid => !Errors.Any();
}
