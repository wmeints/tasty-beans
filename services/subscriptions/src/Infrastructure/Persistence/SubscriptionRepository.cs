using Microsoft.EntityFrameworkCore;
using RecommendCoffee.Subscriptions.Domain.Aggregates.SubscriptionAggregate;
using RecommendCoffee.Subscriptions.Domain.Common;

namespace RecommendCoffee.Subscriptions.Infrastructure.Persistence;

public class SubscriptionRepository: ISubscriptionRepository
{
    private readonly ApplicationDbContext _applicationDbContext;

    public SubscriptionRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<Subscription?> FindByCustomerIdAsync(Guid customerId)
    {
        return await _applicationDbContext.Subscriptions.SingleOrDefaultAsync(x => x.Id == customerId);
    }

    public async Task<PagedResult<Subscription>> FindAll(int pageIndex, int pageSize)
    {
        var totalItemCount = await _applicationDbContext.Subscriptions.LongCountAsync();
        var items = await _applicationDbContext.Subscriptions.OrderBy(x => x.Id)
            .Skip(pageIndex * pageSize).Take(pageSize).ToListAsync();

        return new PagedResult<Subscription>(items, pageIndex, pageSize, totalItemCount);
    }

    public async Task<int> InsertAsync(Subscription subscription)
    {
        await _applicationDbContext.AddAsync(subscription);
        return await _applicationDbContext.SaveChangesAsync();
    }

    public async Task<int> UpdateAsync(Subscription subscription)
    {
        _applicationDbContext.Update(subscription);
        return await _applicationDbContext.SaveChangesAsync();
    }
}