using TastyBeans.Subscriptions.Domain.Aggregates.SubscriptionAggregate;

namespace TastyBeans.Subscriptions.Application.QueryHandlers;

public class FindSubscriptionQueryHandler
{
    private readonly ISubscriptionRepository _subscriptionRepository;

    public FindSubscriptionQueryHandler(ISubscriptionRepository subscriptionRepository)
    {
        _subscriptionRepository = subscriptionRepository;
    }

    public async Task<Subscription?> ExecuteAsync(Guid customerId)
    {
        using var activity = Activities.ExecuteQuery("FindByCustomerId");
        return await _subscriptionRepository.FindByCustomerIdAsync(customerId);
    }
}