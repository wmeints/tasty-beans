using System.Diagnostics.CodeAnalysis;
using RecommendCoffee.Shipping.Domain.Common;

namespace RecommendCoffee.Shipping.Domain.Aggregates.ProductAggregate.Commands;

public record RegisterProductCommandResponse(Product? Product, IEnumerable<ValidationError> Errors)
{
    [MemberNotNullWhen(true, nameof(Product))]
    public bool IsValid => !Errors.Any();
}