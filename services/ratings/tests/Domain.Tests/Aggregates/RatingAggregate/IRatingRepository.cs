using System.Threading.Tasks;
using RecommendCoffee.Ratings.Domain.Aggregates.RatingAggregate;

namespace Domain.Tests.Aggregates.RatingAggregate;

public interface IRatingRepository
{
    Task<int> InsertAsync(Rating rating);
}
