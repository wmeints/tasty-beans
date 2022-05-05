using System.Net.Http.Json;
using TastyBeans.Simulation.Domain.Services.ShippingInformation;

namespace TastyBeans.Simulation.Infrastructure.Agents.Shipping;

public class ShippingServiceAgent: IShippingInformation
{
    private HttpClient _client;

    public ShippingServiceAgent(HttpClient client)
    {
        _client = client;
    }
    
    public async Task<ShippingOrder?> GetShippingOrderAsync(Guid shippingOrderId)
    {
        var response = await _client.GetFromJsonAsync<ShippingOrder>($"/shippingorders/{shippingOrderId}");
        return response;
    }
}