using System.Net.Http.Json;
using TastyBeans.Simulation.Application.Services.Registration;

namespace TastyBeans.Simulation.Infrastructure.Agents;

public class RegistrationServiceAgent: IRegistration
{
    private readonly HttpClient _client;

    public RegistrationServiceAgent(HttpClient client)
    {
        _client = client;
    }
    
    public async Task RegisterCustomerAsync(RegistrationRequest request)
    {
        var response = await _client.PostAsJsonAsync("/", request);
        response.EnsureSuccessStatusCode();
    }
}