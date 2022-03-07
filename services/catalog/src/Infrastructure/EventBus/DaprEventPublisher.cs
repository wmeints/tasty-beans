using System.Reflection;
using Dapr.Client;
using RecommendCoffee.Catalog.Application.Common;
using RecommendCoffee.Catalog.Domain.Common;

namespace RecommendCoffee.Catalog.Infrastructure.EventBus;

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
                topic?.Name ?? "catalog.deadletter.v1",
                evt);
        }
    }
}