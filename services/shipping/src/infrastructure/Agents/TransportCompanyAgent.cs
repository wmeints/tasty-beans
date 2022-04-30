using Dapr.Client;
using Microsoft.Extensions.Logging;
using RecommendCoffee.Shipping.Application.Services;

namespace RecommendCoffee.Shipping.Infrastructure.Agents;

public class TransportCompanyAgent : ITransportCompany
{
    private readonly DaprClient _daprClient;
    private readonly ILogger<TransportCompanyAgent> _logger;

    public TransportCompanyAgent(DaprClient daprClient, ILogger<TransportCompanyAgent> logger)
    {
        _daprClient = daprClient;
        _logger = logger;
    }

    public async Task ShipAsync(Guid shippingOrderId)
    {
        try
        {
            await _daprClient.InvokeMethodAsync("transport", "orders", new { shippingOrderId });
        }
        catch (InvocationException ex)
        {
            var responseContent = await ex.Response.Content.ReadAsStringAsync();
            
            _logger.LogError(ex,
                "Failed to send shipping order to transport company. Got response {StatusCode}: {ResponseContent}",
                ex.Response.StatusCode, responseContent);

            throw;
        }
    }
}