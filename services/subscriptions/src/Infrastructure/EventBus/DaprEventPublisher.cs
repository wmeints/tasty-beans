using System.Reflection;
using Dapr.Client;
using RecommendCoffee.Subscriptions.Application.Common;
using RecommendCoffee.Subscriptions.Domain.Common;

namespace RecommendCoffee.Subscriptions.Infrastructure.EventBus;

public class DaprEventPublisher : IEventPublisher
{
    private readonly DaprClient _daprClient;

    public DaprEventPublisher(DaprClient daprClient)
    {
        _daprClient = daprClient;
    }

    public async Task PublishEventsAsync(IEnumerable<IDomainEvent> events)
    {
        foreach (var evt in events)
        {
            var topic = evt.GetType().GetCustomAttribute<TopicAttribute>();
            await _daprClient.PublishEventAsync<object>(
                "pubsub", 
                topic?.Name ?? "subscriptions.deadletter.v1", 
                evt);

            Metrics.EventsPublished.Add(1);
        }
    }
}