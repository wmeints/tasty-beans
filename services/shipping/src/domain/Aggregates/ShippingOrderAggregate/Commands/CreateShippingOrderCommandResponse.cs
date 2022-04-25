using System.Diagnostics.CodeAnalysis;

namespace RecommendCoffee.Shipping.Domain.Aggregates.ShippingOrderAggregate.Commands;

public record CreateShippingOrderCommandResponse(
    ShippingOrder? Order,
    IEnumerable<ValidationError> Errors,
    IEnumerable<IDomainEvent> Events)
{
    [MemberNotNullWhen(true, nameof(Order))]
    public bool IsValid => !Errors.Any();
}
