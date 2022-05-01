using System.Net.Http.Json;
using Dapr.Client;
using Microsoft.Extensions.Logging;
using RecommendCoffee.Shipping.Application.Services;

namespace RecommendCoffee.Shipping.Infrastructure.Agents;

public class TransportCompanyAgent : ITransportCompany
{
    private readonly HttpClient _client;
    private readonly ILogger<TransportCompanyAgent> _logger;

    public TransportCompanyAgent(HttpClient client, ILogger<TransportCompanyAgent> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task ShipAsync(Guid shippingOrderId)
    {
        var response = await _client.PostAsJsonAsync("/orders", new {shippingOrderId});

        if (!response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            
            _logger.LogError("Failed to execute request. Got response {StatusCode}: {ResponseContent}",
                response.StatusCode, responseContent);
        }

        // Make sure we get an exception up the call chain.
        // Otherwise the app doesn't tell the client that something failed.
        response.EnsureSuccessStatusCode();
    }
}