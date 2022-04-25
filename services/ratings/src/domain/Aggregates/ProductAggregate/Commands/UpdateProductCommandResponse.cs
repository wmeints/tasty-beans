namespace RecommendCoffee.Ratings.Domain.Aggregates.ProductAggregate.Commands;

public record UpdateProductCommandResponse(IEnumerable<ValidationError> Errors)
{
    public bool IsValid => !Errors.Any();
}