using System.Threading.Tasks;
using TastyBeans.Ratings.Domain.Aggregates.RatingAggregate;

namespace TastyBeans.Ratings.Domain.Tests.Aggregates.RatingAggregate;

public interface IRatingRepository
{
    Task<int> InsertAsync(Rating rating);
}
