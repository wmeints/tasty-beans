using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RecommendCoffee.Subscriptions.Application.Common;
using RecommendCoffee.Subscriptions.Application.IntegrationEvents;
using RecommendCoffee.Subscriptions.Domain.Aggregates.SubscriptionAggregate;
using RecommendCoffee.Subscriptions.Domain.Aggregates.SubscriptionAggregate.Commands;
using RecommendCoffee.Subscriptions.Domain.Services.Recommendations;
using RecommendCoffee.Subscriptions.Domain.Services.Shipping;

namespace RecommendCoffee.Subscriptions.Application.EventHandlers;

public class MonthHasPassedEventHandler
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<MonthHasPassedEventHandler> _logger;
    
    public MonthHasPassedEventHandler(ILogger<MonthHasPassedEventHandler> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public async ValueTask HandleAsync(MonthHasPassedEvent evt)
    {
        // IMPORTANT: This event handler creates its own scope because it's launched from the background task queue.
        // Without this scope it would use disposed objects or things from a scope that it shouldn't be using.
        // Using objects from a scope that is disposed can cause serious trouble.
        
        await using var scope = _serviceProvider.CreateAsyncScope();

        var subscriptionRepository = scope.ServiceProvider.GetRequiredService<ISubscriptionRepository>();
        var recommendations = scope.ServiceProvider.GetRequiredService<IRecommendations>();
        var shipping = scope.ServiceProvider.GetRequiredService<IShipping>();
        var eventPublisher = scope.ServiceProvider.GetRequiredService<IEventPublisher>();
        
        var subscriptions = await subscriptionRepository.FindAllMonthlySubscriptions();

        foreach (var subscription in subscriptions)
        {
            var result = await subscription.CreateShipment(
                new CreateShipmentCommand(), recommendations, shipping);

            if (!result.IsValid)
            {
                _logger.LogWarning("Could not generate a shipment for subscription {SubscriptionId}: {Errors}", 
                    subscription.Id, result.Errors.First().ErrorMessage);
            }

            await eventPublisher.PublishEventsAsync(result.Events);
        }
    }
}