using TastyBeans.Shared.Domain;

namespace TastyBeans.Subscriptions.Domain.Aggregates.SubscriptionAggregate;

public interface ISubscriptionRepository
{
    Task<Subscription?> FindByCustomerIdAsync(Guid customerId);
    Task<PagedResult<Subscription>> FindAll(int pageIndex, int pageSize);
    Task<int> InsertAsync(Subscription subscription);
    Task<int> UpdateAsync(Subscription subscription);
    Task<IEnumerable<Subscription>> FindAllMonthlySubscriptions();
}