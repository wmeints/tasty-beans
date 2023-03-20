using Microsoft.EntityFrameworkCore;
using TastyBeans.Shared.Domain;
using TastyBeans.Subscriptions.Domain.Aggregates.SubscriptionAggregate;

namespace TastyBeans.Subscriptions.Infrastructure.Persistence;

public class SubscriptionRepository: ISubscriptionRepository
{
    private readonly ApplicationDbContext _applicationDbContext;

    public SubscriptionRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<Subscription?> FindByCustomerIdAsync(Guid customerId)
    {
        using var activity = Activities.ExecuteDatabaseCommand();
        
        return await _applicationDbContext.Subscriptions.SingleOrDefaultAsync(x => x.Id == customerId);
    }

    public async Task<PagedResult<Subscription>> FindAll(int pageIndex, int pageSize)
    {
        using var activity = Activities.ExecuteDatabaseCommand();
        
        var totalItemCount = await _applicationDbContext.Subscriptions.LongCountAsync();
        var items = await _applicationDbContext.Subscriptions.OrderBy(x => x.Id)
            .Skip(pageIndex * pageSize).Take(pageSize).ToListAsync();

        return new PagedResult<Subscription>(items, pageIndex, pageSize, totalItemCount);
    }

    public async Task<int> InsertAsync(Subscription subscription)
    {
        using var activity = Activities.ExecuteDatabaseCommand();
        
        await _applicationDbContext.AddAsync(subscription);
        return await _applicationDbContext.SaveChangesAsync();
    }

    public async Task<int> UpdateAsync(Subscription subscription)
    {
        using var activity = Activities.ExecuteDatabaseCommand();
        
        _applicationDbContext.Update(subscription);
        return await _applicationDbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<Subscription>> FindAllMonthlySubscriptions()
    {
        return await _applicationDbContext.Subscriptions
            .Where(x => x.ShippingFrequency == ShippingFrequency.Monthly)
            .ToListAsync();
    }
}