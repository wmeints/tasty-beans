using System.Diagnostics.CodeAnalysis;
using RecommendCoffee.Shipping.Domain.Common;

namespace RecommendCoffee.Shipping.Domain.Aggregates.ShippingOrderAggregate.Commands;

public record CreateShippingOrderCommandResponse(
    ShippingOrder? Order,
    IEnumerable<ValidationError> Errors,
    IEnumerable<IDomainEvent> Events)
{
    [MemberNotNullWhen(true, nameof(Order))]
    public bool IsValid => !Errors.Any();
}
