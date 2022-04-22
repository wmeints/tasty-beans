using Dapr.Client;
using RecommendCoffee.Subscriptions.Domain.Services.Shipping;
using RecommendCoffee.Subscriptions.Domain.Services.Shipping.Commands;

namespace RecommendCoffee.Subscriptions.Infrastructure.Agents.Shipping;

public class ShippingAgent: IShipping
{
    private readonly DaprClient _daprClient;

    public ShippingAgent(DaprClient daprClient)
    {
        _daprClient = daprClient;
    }

    public async Task CreateShippingOrderAsync(CreateShippingOrderCommand cmd)
    {
        var shippingData = new CreateShippingOrderRequest(cmd.CustomerId, new[] { new OrderItem(cmd.ProductId, 1) });
        await _daprClient.InvokeMethodAsync(HttpMethod.Post, "shipping", "shippingorders", shippingData);
    }
}