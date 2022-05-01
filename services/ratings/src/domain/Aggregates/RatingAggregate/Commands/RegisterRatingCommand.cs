namespace TastyBeans.Ratings.Domain.Aggregates.RatingAggregate.Commands;

public record RegisterRatingCommand(Guid CustomerId, Guid ProductId, int Value);