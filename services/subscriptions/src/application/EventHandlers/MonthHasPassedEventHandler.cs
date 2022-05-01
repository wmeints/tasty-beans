using Microsoft.Extensions.Logging;
using TastyBeans.Shared.Application;
using TastyBeans.Subscriptions.Application.IntegrationEvents;
using TastyBeans.Subscriptions.Domain.Aggregates.SubscriptionAggregate;
using TastyBeans.Subscriptions.Domain.Aggregates.SubscriptionAggregate.Commands;
using TastyBeans.Subscriptions.Domain.Services.Recommendations;
using TastyBeans.Subscriptions.Domain.Services.Shipping;

namespace TastyBeans.Subscriptions.Application.EventHandlers;

public class MonthHasPassedEventHandler
{
    private readonly ILogger<MonthHasPassedEventHandler> _logger;
    private ISubscriptionRepository _subscriptionRepository;
    private IRecommendations _recommendations;
    private IShipping _shipping;
    private IEventPublisher _eventPublisher;

    public MonthHasPassedEventHandler(
        ILogger<MonthHasPassedEventHandler> logger,
        ISubscriptionRepository subscriptionRepository, 
        IRecommendations recommendations, IShipping shipping,
        IEventPublisher eventPublisher)
    {
        _logger = logger;
        _subscriptionRepository = subscriptionRepository;
        _recommendations = recommendations;
        _shipping = shipping;
        _eventPublisher = eventPublisher;
    }

    public async ValueTask HandleAsync(MonthHasPassedEvent evt)
    {
        var subscriptions = await _subscriptionRepository.FindAllMonthlySubscriptions();

        foreach (var subscription in subscriptions)
        {
            var result = await subscription.CreateShipment(
                new CreateShipmentCommand(), _recommendations, _shipping);

            if (!result.IsValid)
            {
                _logger.LogWarning("Could not generate a shipment for subscription {SubscriptionId}: {Errors}",
                    subscription.Id, result.Errors.First().ErrorMessage);
            }

            await _eventPublisher.PublishEventsAsync(result.Events);
        }
    }
}