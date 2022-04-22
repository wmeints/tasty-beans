using Dapr.Client;
using RecommendCoffee.Subscriptions.Domain.Services.Recommendations;

namespace RecommendCoffee.Subscriptions.Infrastructure.Agents.Recommendations;

public class RecommendationsAgent : IRecommendations
{
    private readonly DaprClient _daprClient;

    public RecommendationsAgent(DaprClient daprClient)
    {
        _daprClient = daprClient;
    }

    public async Task<Guid> GetRecommendProductAsync(Guid customerId)
    {
        var response =
            await _daprClient.InvokeMethodAsync<GenerateRecommendationRequest, GenerateRecommendationResponse>(
                HttpMethod.Post, "recommendations", "recommendation",
                new GenerateRecommendationRequest(customerId));

        return response.ProductId;
    }
}