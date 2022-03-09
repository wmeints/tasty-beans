using RecommendCoffee.Payments.Domain.Common;

namespace RecommendCoffee.Payments.Domain.Aggregates.PaymentMethodAggregate.Commands;

public record RegisterPaymentMethodReply(
    PaymentMethod? PaymentMethod, 
    IEnumerable<ValidationError> Errors,
    IEnumerable<IDomainEvent> Events)
{
    public bool IsValid => !Errors.Any();
}
