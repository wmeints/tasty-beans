using TastyBeans.Simulation.Domain.Services.Subscriptions;

namespace TastyBeans.Simulation.Infrastructure.Agents.Subscriptions;

public class SubscriptionsServiceAgent: ISubscriptions
{
    private readonly HttpClient _client;

    public SubscriptionsServiceAgent(HttpClient client)
    {
        _client = client;
    }
    
    public async Task CancelSubscriptionAsync(Guid customerId)
    {
        var response = await _client.DeleteAsync($"/{customerId}");
        response.EnsureSuccessStatusCode();
    }
}