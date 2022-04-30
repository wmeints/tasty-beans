using System.Net;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Dapr.Client;
using Microsoft.Extensions.Logging;
using RecommendCoffee.Subscriptions.Domain.Services.Shipping;
using RecommendCoffee.Subscriptions.Domain.Services.Shipping.Commands;

namespace RecommendCoffee.Subscriptions.Infrastructure.Agents.Shipping;

public class ShippingAgent : IShipping
{
    private readonly DaprClient _daprClient;
    private readonly ILogger<ShippingAgent> _logger;

    public ShippingAgent(DaprClient daprClient, ILogger<ShippingAgent> logger)
    {
        _daprClient = daprClient;
        _logger = logger;
    }

    public async Task CreateShippingOrderAsync(CreateShippingOrderCommand cmd)
    {
        var shippingData = new CreateShippingOrderRequest(cmd.CustomerId, new[] { new OrderItem(cmd.ProductId, 1) });

        try
        {
            await _daprClient.InvokeMethodAsync(HttpMethod.Post, "shipping", "shippingorders", shippingData);
        }
        catch (InvocationException ex)
        {
            var responseData = await ex.Response.Content.ReadAsStringAsync();

            _logger.LogError(ex, "Failed to invoke shipping service. Got response {StatusCode}: {ResponseData}",
                ex.Response.GetType(), responseData);

            throw ex;
        }
    }
}