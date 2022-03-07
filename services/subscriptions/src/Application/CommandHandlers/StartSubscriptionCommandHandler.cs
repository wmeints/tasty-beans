using Microsoft.Extensions.Logging;
using RecommendCoffee.Subscriptions.Application.Common;
using RecommendCoffee.Subscriptions.Domain.Aggregates.SubscriptionAggregate;
using RecommendCoffee.Subscriptions.Domain.Aggregates.SubscriptionAggregate.Commands;

namespace RecommendCoffee.Subscriptions.Application.CommandHandlers;

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
            }
            else
            {
                _logger.LogWarning("The request is invalid");
                
                foreach (var error in response.Errors)
                {
                    _logger.LogWarning("Validation of property {PropertyName} failed: {ErrorMessage}",
                        error.PropertyPath,error.ErrorMessage);
                }
            }
        }
        else
        {
            _logger.LogInformation("Starting new subscription");
            
            response = Subscription.Start(cmd);
            subscription = response.Subscription;

            if (response.IsValid)
            {
                await _subscriptionRepository.InsertAsync(response.Subscription);
                await _eventPublisher.PublishEventsAsync(response.Events);
            }
            else
            {
                _logger.LogWarning("The request is invalid");

                foreach (var error in response.Errors)
                {
                    _logger.LogWarning("Validation of property {PropertyName} failed: {ErrorMessage}",
                        error.PropertyPath,error.ErrorMessage);
                }
            }
        }

        return response;
    }
}