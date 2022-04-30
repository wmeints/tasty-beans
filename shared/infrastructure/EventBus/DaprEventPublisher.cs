using System.Reflection;
using Dapr.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RecommendCoffee.Shared.Domain;
using RecommendCoffee.Shared.Application;

namespace RecommendCoffee.Shared.Infrastructure.EventBus;

public class DaprEventPublisher : IEventPublisher
{
    private readonly ILogger<DaprEventPublisher> _logger;
    private readonly IOptions<DaprEventPublisherOptions> _options;
    private readonly DaprClient _daprClient;

    public DaprEventPublisher(DaprClient daprClient, ILogger<DaprEventPublisher> logger, IOptions<DaprEventPublisherOptions> options)
    {
        _daprClient = daprClient;
        _logger = logger;
        _options = options;
    }

    public async Task PublishEventsAsync(IEnumerable<IDomainEvent> events)
    {
        foreach (var evt in events)
        {
            var topic = evt.GetType().GetCustomAttribute<TopicAttribute>();

            if (topic == null)
            {
                _logger.LogWarning(
                    "Missing topic attribute on event {eventType}. It will be published on the dead letter topic.", 
                    evt.GetType().Name);
            }
            
            await _daprClient.PublishEventAsync<object>("pubsub", topic?.Name ?? _options.Value.DeadLetterTopic, evt);
        }
    }
}