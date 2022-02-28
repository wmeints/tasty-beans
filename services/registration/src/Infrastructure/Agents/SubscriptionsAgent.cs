using Dapr.Client;
using RecommendCoffee.Registration.Domain.Subscriptions;

namespace RecommendCoffee.Registration.Infrastructure.Agents;

public class SubscriptionsAgent: ISubscriptions
{
    private readonly DaprClient _daprClient;

    public SubscriptionsAgent(DaprClient daprClient)
    {
        _daprClient = daprClient;
    }

    public async Task RegisterSubscriptionAsync(RegisterSubscriptionRequest request)
    {
        await _daprClient.InvokeMethodAsync("subscriptions", "subscriptions", request);
    }
}