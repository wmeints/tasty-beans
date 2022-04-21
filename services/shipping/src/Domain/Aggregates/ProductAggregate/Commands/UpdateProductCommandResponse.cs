using RecommendCoffee.Shipping.Domain.Common;

namespace RecommendCoffee.Shipping.Domain.Aggregates.ProductAggregate.Commands;

public record UpdateProductCommandResponse(IEnumerable<ValidationError> Errors)
{
    public bool IsValid => !Errors.Any();
}