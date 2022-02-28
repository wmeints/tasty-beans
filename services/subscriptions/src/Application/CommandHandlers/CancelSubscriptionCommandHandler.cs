using RecommendCoffee.Subscriptions.Application.Common;
using RecommendCoffee.Subscriptions.Domain.Aggregates.SubscriptionAggregate;
using RecommendCoffee.Subscriptions.Domain.Aggregates.SubscriptionAggregate.Commands;
using RecommendCoffee.Subscriptions.Domain.Common;

namespace RecommendCoffee.Subscriptions.Application.CommandHandlers;

public class CancelSubscriptionCommandHandler
{
    private readonly ISubscriptionRepository _subscriptionRepository;
    private readonly IEventPublisher _eventPublisher;

    public CancelSubscriptionCommandHandler(ISubscriptionRepository subscriptionRepository, IEventPublisher eventPublisher)
    {
        _subscriptionRepository = subscriptionRepository;
        _eventPublisher = eventPublisher;
    }

    public async Task<CancelSubscriptionCommandReply> ExecuteAsync(CancelSubscriptionCommand cmd)
    {
        var subscription = await _subscriptionRepository.FindByCustomerIdAsync(cmd.CustomerId);

        if (subscription == null)
        {
            throw new AggregateNotFoundException($"Can't find subscription for customer {cmd.CustomerId}");
        }

        var response = subscription.Cancel(cmd);

        if (response.IsValid)
        {
            await _subscriptionRepository.UpdateAsync(subscription);
            await _eventPublisher.PublishEventsAsync(response.Events);
        }

        return response;
    }
}