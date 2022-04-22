using RecommendCoffee.Recommendations.Domain.Common;

namespace RecommendCoffee.Recommendations.Domain.Aggregates.ProductAggregate.Commands;

public record UpdateProductCommandResponse(IEnumerable<ValidationError> Errors)
{
    public bool IsValid => !Errors.Any();
}