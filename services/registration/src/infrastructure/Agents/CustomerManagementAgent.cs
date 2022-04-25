using Dapr.Client;
using Microsoft.Extensions.Logging;
using RecommendCoffee.Registration.Domain.Customers;

namespace RecommendCoffee.Registration.Infrastructure.Agents;

public class CustomerManagementAgent: ICustomerManagement
{
    private readonly DaprClient _daprClient;
    private readonly ILogger<CustomerManagementAgent> _logger;

    public CustomerManagementAgent(DaprClient daprClient, ILogger<CustomerManagementAgent> logger)
    {
        _daprClient = daprClient;
        _logger = logger;
    }

    public async Task RegisterCustomerAsync(RegisterCustomerRequest request)
    {
        _logger.LogInformation("Registering new customer");
        await _daprClient.InvokeMethodAsync("customermanagement", "customers", request);
    }
}