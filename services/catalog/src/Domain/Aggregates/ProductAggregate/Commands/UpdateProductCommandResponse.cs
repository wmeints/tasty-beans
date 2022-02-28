using System.Diagnostics.CodeAnalysis;
using RecommendCoffee.Catalog.Domain.Common;

namespace RecommendCoffee.Catalog.Domain.Aggregates.ProductAggregate.Commands;

public record UpdateProductCommandResponse(IEnumerable<ValidationError> Errors, IEnumerable<IDomainEvent> Events)
{
    public bool IsValid => !Errors.Any();
}