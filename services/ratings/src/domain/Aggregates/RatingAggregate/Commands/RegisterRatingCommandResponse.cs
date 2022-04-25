using System.Diagnostics.CodeAnalysis;

namespace RecommendCoffee.Ratings.Domain.Aggregates.RatingAggregate.Commands;

public record RegisterRatingCommandResponse(Rating? Rating, IEnumerable<ValidationError> Errors, IEnumerable<IDomainEvent> Events)
{
    [MemberNotNullWhen(true, nameof(Rating))]
    public bool IsValid => !Errors.Any();
}