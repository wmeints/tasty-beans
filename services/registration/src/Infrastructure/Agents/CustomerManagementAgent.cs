using Dapr.Client;
using RecommendCoffee.Registration.Domain.Customers;

namespace RecommendCoffee.Registration.Infrastructure.Agents;

public class CustomerManagementAgent: ICustomerManagement
{
    private readonly DaprClient _daprClient;

    public CustomerManagementAgent(DaprClient daprClient)
    {
        _daprClient = daprClient;
    }

    public async Task RegisterCustomerAsync(RegisterCustomerRequest request)
    {
        await _daprClient.InvokeMethodAsync("customers", "customers", request);
    }
}