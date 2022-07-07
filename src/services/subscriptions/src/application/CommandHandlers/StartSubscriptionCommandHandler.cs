using Microsoft.Extensions.Logging;
using TastyBeans.Shared.Application;
using TastyBeans.Subscriptions.Domain.Aggregates.SubscriptionAggregate;
using TastyBeans.Subscriptions.Domain.Aggregates.SubscriptionAggregate.Commands;

namespace TastyBeans.Subscriptions.Application.CommandHandlers;

public class StartSubscriptionCommandHandler
{
    private readonly ILogger<StartSubscriptionCommandHandler> _logger;
    private readonly IEventPublisher _eventPublisher;
    private readonly ISubscriptionRepository _subscriptionRepository;

    public StartSubscriptionCommandHandler(IEventPublisher eventPublisher,
        ISubscriptionRepository subscriptionRepository, 
        ILogger<StartSubscriptionCommandHandler> logger)
    {
        _eventPublisher = eventPublisher;
        _subscriptionRepository = subscriptionRepository;
        _logger = logger;
    }

    public async Task<StartSubscriptionCommandReply> ExecuteAsync(StartSubscriptionCommand cmd)
    {
        using var activity = Activities.ExecuteCommand("StartSubscription");
        
        StartSubscriptionCommandReply response;
        Subscription? subscription = await _subscriptionRepository.FindByCustomerIdAsync(cmd.CustomerId);

        if (subscription != null)
        {
            _logger.LogInformation("Updating existing subscription");
            
            response = subscription.Resubscribe(cmd);

            if (response.IsValid)
            {
                await _subscriptionRepository.UpdateAsync(subscription);
                await _eventPublisher.PublishEventsAsync(response.Events);
                
                Metrics.SubscriptionsStarted.Add(1);
            }
        }
        else
        {
            _logger.LogInformation("Starting new subscription");
            
            response = Subscription.Start(cmd);

            if (response.IsValid)
            {
                await _subscriptionRepository.InsertAsync(response.Subscription);
                await _eventPublisher.PublishEventsAsync(response.Events);

                Metrics.SubscriptionsStarted.Add(1);
            }
        }

        return response;
    }
}