using System.Diagnostics.CodeAnalysis;
using RecommendCoffee.Ratings.Domain.Common;

namespace RecommendCoffee.Ratings.Domain.Aggregates.ProductAggregate.Commands;

public record RegisterProductCommandResponse(Product? Product, IEnumerable<ValidationError> Errors)
{
    [MemberNotNullWhen(true, nameof(Product))]
    public bool IsValid => !Errors.Any();
}