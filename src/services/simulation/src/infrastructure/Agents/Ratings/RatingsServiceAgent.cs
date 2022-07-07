using System.Net.Http.Json;
using TastyBeans.Simulation.Domain.Services.Ratings;

namespace TastyBeans.Simulation.Infrastructure.Agents.Ratings;

public class RatingsServiceAgent: IRatings
{
    private readonly HttpClient _client;

    public RatingsServiceAgent(HttpClient client)
    {
        _client = client;
    }

    public async Task RateProductAsync(Guid customerId, Guid productId, int value)
    {
        await _client.PostAsJsonAsync("/ratings", new { customerId, productId, value});
    }
}