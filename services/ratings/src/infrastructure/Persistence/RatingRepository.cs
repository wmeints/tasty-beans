using TastyBeans.Ratings.Domain.Aggregates.RatingAggregate;

namespace TastyBeans.Ratings.Infrastructure.Persistence;

public class RatingRepository : IRatingRepository
{
    private readonly ApplicationDbContext _applicationDbContext;

    public RatingRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<int> InsertAsync(Rating rating)
    {
        await _applicationDbContext.Ratings.AddAsync(rating);
        return await _applicationDbContext.SaveChangesAsync();
    }
}
