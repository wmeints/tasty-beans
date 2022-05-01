using Dapr.Client;
using Microsoft.Extensions.Logging;
using TastyBeans.Subscriptions.Domain.Services.Recommendations;

namespace TastyBeans.Subscriptions.Infrastructure.Agents.Recommendations;

public class RecommendationsAgent : IRecommendations
{
    private readonly DaprClient _daprClient;
    private readonly ILogger<RecommendationsAgent> _logger;

    public RecommendationsAgent(DaprClient daprClient, ILogger<RecommendationsAgent> logger)
    {
        _daprClient = daprClient;
        _logger = logger;
    }

    public async Task<Guid> GetRecommendProductAsync(Guid customerId)
    {
        try
        {
            var response = await _daprClient.InvokeMethodAsync<GenerateRecommendationRequest, GenerateRecommendationResponse>(
                HttpMethod.Post, "recommendations", "recommendation",
                new GenerateRecommendationRequest(customerId));

            return response.ProductId;
        }
        catch (InvocationException ex)
        {
            var responseData = await ex.Response.Content.ReadAsStringAsync();

            _logger.LogError(ex, "Failed to invoke recommendations service. Got response {StatusCode}: {ResponseData}",
                ex.Response.GetType(), responseData);

            throw ex;
        }
    }
}