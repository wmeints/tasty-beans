using System.Diagnostics.Metrics;
using System.Reflection;
using Dapr.Client;
using RecommendCoffee.Catalog.Application.Common;
using RecommendCoffee.Catalog.Domain.Common;

namespace RecommendCoffee.Catalog.Infrastructure.EventBus;

public class DaprEventPublisher : IEventPublisher
{
    private const string DEADLETTER_TOPIC = "catalog.deadletter.v1";
    
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
            
            using (var activity = Activities.PublishEvent(topic?.Name ?? DEADLETTER_TOPIC))
            {
                await _daprClient.PublishEventAsync<object>(
                    "pubsub",
                    topic?.Name ?? DEADLETTER_TOPIC,
                    evt);

                Metrics.EventsPublished.Add(1);
            }
        }
    }
}