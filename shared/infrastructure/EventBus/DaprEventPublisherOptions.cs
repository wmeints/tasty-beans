namespace RecommendCoffee.Shared.Infrastructure.EventBus;

public class DaprEventPublisherOptions
{
    public string DeadLetterTopic { get; set; } = "deadletter.v1";
}