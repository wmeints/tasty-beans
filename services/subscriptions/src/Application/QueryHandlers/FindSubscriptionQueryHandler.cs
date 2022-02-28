using RecommendCoffee.Subscriptions.Domain.Aggregates.SubscriptionAggregate;

namespace RecommendCoffee.Subscriptions.Application.QueryHandlers;

public class FindSubscriptionQueryHandler
{
    private readonly ISubscriptionRepository _subscriptionRepository;

    public FindSubscriptionQueryHandler(ISubscriptionRepository subscriptionRepository)
    {
        _subscriptionRepository = subscriptionRepository;
    }

    public async Task<Subscription?> ExecuteAsync(Guid customerId)
    {
        return await _subscriptionRepository.FindByCustomerIdAsync(customerId);
    }
}