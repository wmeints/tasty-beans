using TastyBeans.Shared.Domain;

namespace TastyBeans.Recommendations.Domain.Aggregates.ProductAggregate.Commands;

public record UpdateProductCommandResponse(IEnumerable<ValidationError> Errors)
{
    public bool IsValid => !Errors.Any();
}