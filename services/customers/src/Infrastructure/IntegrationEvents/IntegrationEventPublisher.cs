using Dapr.Client;
using Microsoft.Extensions.Options;
using RecommendCoffee.Customers.Infrastructure.IntegrationEvents;
using RecommendCoffee.Customers.Application.IntegrationEvents;
using RecommendCoffee.Customers.Domain.Common;
using System.Reflection;

namespace RecommendCoffee.Customers.Infrastructure.IntegrationEvents;

public class IntegrationEventPublisher : IIntegrationEventPublisher
{
    private DaprClient _daprClient;
    private readonly IOptions<IntegrationEventPublisherOptions> options;

    public IntegrationEventPublisher(DaprClient daprClient, IOptions<IntegrationEventPublisherOptions> options)
    {
        _daprClient = daprClient;
        this.options = options;
    }

    public async Task PublishAsync(IEnumerable<Event> events)
    {
        foreach (var evt in events)
        {
            var topicName = evt.GetType().GetCustomAttributes<TopicAttribute>().SingleOrDefault()?.TopicName ?? "misc.fct.undeliverable.1";
            await _daprClient.PublishEventAsync(options.Value.PubSubName, topicName, evt);
        }
    }
}
