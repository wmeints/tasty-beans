using System.Reflection;
using System.Text.Json;
using Azure.Messaging.ServiceBus;
using RecommendCoffee.Catalog.Application.Common;
using RecommendCoffee.Catalog.Domain.Common;

namespace Infrastructure.EventBus;

public class AzureEventBusPublisher: IEventPublisher
{
    private readonly ServiceBusClient _client;

    public AzureEventBusPublisher(ServiceBusClient client)
    {
        _client = client;
    }

    public async Task PublishEventsAsync(IEnumerable<IDomainEvent> events)
    {
        foreach (var evt in events)
        {
            var topic = evt.GetType().GetCustomAttribute<TopicAttribute>();
            var sender = _client.CreateSender(topic.Name ?? "catalog.deadletter.v1");

            var serializedData = JsonSerializer.Serialize(evt);
            var message = new ServiceBusMessage(JsonSerializer.Serialize(evt));
            
            await sender.SendMessageAsync(message);
        }
    }
}