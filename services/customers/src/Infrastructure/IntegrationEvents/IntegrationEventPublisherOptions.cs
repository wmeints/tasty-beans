namespace RecommendCoffee.Customers.Infrastructure.IntegrationEvents;

public class IntegrationEventPublisherOptions
{
    public string PubSubName { get; set; } = "pubsub";
}
