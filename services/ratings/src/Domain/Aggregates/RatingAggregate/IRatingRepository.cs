namespace RecommendCoffee.Ratings.Domain.Aggregates.RatingAggregate;

public interface IRatingRepository
{
    Task<int> InsertAsync(Rating rating);
}
