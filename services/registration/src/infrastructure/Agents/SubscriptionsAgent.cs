using Dapr.Client;
using Microsoft.Extensions.Logging;
using TastyBeans.Registration.Domain.Subscriptions;

namespace TastyBeans.Registration.Infrastructure.Agents;

public class SubscriptionsAgent: ISubscriptions
{
    private readonly DaprClient _daprClient;
    private readonly ILogger<SubscriptionsAgent> _logger;

    public SubscriptionsAgent(DaprClient daprClient, ILogger<SubscriptionsAgent> logger)
    {
        _daprClient = daprClient;
        _logger = logger;
    }

    public async Task RegisterSubscriptionAsync(RegisterSubscriptionRequest request)
    {
        _logger.LogInformation("Registering new subscription");
        await _daprClient.InvokeMethodAsync("subscriptions", "subscriptions", request);
    }
}