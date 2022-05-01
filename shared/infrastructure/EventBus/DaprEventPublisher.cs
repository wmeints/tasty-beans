using System.Reflection;
using Dapr.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TastyBeans.Shared.Application;
using TastyBeans.Shared.Domain;

namespace TastyBeans.Shared.Infrastructure.EventBus;

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

            try
            {
                await _daprClient.PublishEventAsync<object>("pubsub", topic?.Name ?? _options.Value.DeadLetterTopic,
                    evt);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(
                    "A problem happened while publishing the event. This could be caused by the target application raising an error. Original error was: {Error}", 
                    ex);
            }
        }
    }
}