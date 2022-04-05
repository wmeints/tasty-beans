using System.Threading.Tasks;

namespace Domain.Tests.Aggregates.RatingAggregate;

public interface IRatingRepository
{
    Task<int> InsertAsync(Rating rating);
}
