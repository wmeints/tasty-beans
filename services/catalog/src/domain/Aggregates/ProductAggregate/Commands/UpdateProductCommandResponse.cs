using TastyBeans.Shared.Domain;

namespace TastyBeans.Catalog.Domain.Aggregates.ProductAggregate.Commands;

public record UpdateProductCommandResponse(IEnumerable<ValidationError> Errors, IEnumerable<IDomainEvent> Events)
{
    public bool IsValid => !Errors.Any();
}