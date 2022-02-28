using RecommendCoffee.Subscriptions.Application.Common;
using RecommendCoffee.Subscriptions.Domain.Aggregates.SubscriptionAggregate;
using RecommendCoffee.Subscriptions.Domain.Aggregates.SubscriptionAggregate.Commands;

namespace RecommendCoffee.Subscriptions.Application.CommandHandlers;

public class StartSubscriptionCommandHandler
{
    private readonly IEventPublisher _eventPublisher;
    private readonly ISubscriptionRepository _subscriptionRepository;

    public StartSubscriptionCommandHandler(IEventPublisher eventPublisher,
        ISubscriptionRepository subscriptionRepository)
    {
        _eventPublisher = eventPublisher;
        _subscriptionRepository = subscriptionRepository;
    }

    public async Task<StartSubscriptionCommandReply> ExecuteAsync(StartSubscriptionCommand cmd)
    {
        StartSubscriptionCommandReply response;
        Subscription? subscription = await _subscriptionRepository.FindByCustomerIdAsync(cmd.CustomerId);

        if (subscription != null)
        {
            response = subscription.Resubscribe(cmd);

            if (response.IsValid)
            {
                await _subscriptionRepository.UpdateAsync(subscription);
                await _eventPublisher.PublishEventsAsync(response.Events);
            }
        }
        else
        {
            response = Subscription.Start(cmd);
            subscription = response.Subscription;

            if (response.IsValid)
            {
                await _subscriptionRepository.InsertAsync(response.Subscription);
                await _eventPublisher.PublishEventsAsync(response.Events);
            }
        }

        return response;
    }
}