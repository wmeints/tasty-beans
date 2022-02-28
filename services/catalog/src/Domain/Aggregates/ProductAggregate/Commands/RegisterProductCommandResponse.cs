using System.Diagnostics.CodeAnalysis;
using Domain.Aggregates.ProductAggregate;
using RecommendCoffee.Catalog.Domain.Common;

namespace RecommendCoffee.Catalog.Domain.Aggregates.ProductAggregate.Commands;

public record RegisterProductCommandResponse(
    Product? Product,
    IEnumerable<ValidationError> Errors,
    IEnumerable<IDomainEvent> Events)
{
    [MemberNotNullWhen(true, nameof(Product))]
    public bool IsValid => !Errors.Any();
}