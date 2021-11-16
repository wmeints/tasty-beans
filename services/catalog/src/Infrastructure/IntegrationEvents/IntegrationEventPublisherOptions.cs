namespace RecommendCoffee.Catalog.Infrastructure.IntegrationEvents;

public class IntegrationEventPublisherOptions
{
    public string PubSubName { get; set; } = "pubsub";
}
