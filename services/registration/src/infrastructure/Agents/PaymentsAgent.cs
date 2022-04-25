using Dapr.Client;
using Microsoft.Extensions.Logging;
using RecommendCoffee.Registration.Domain.Payments;

namespace RecommendCoffee.Registration.Infrastructure.Agents;

public class PaymentsAgent: IPayments
{
    private readonly DaprClient _daprClient;
    private readonly ILogger<PaymentsAgent> _logger;

    public PaymentsAgent(DaprClient daprClient, ILogger<PaymentsAgent> logger)
    {
        _daprClient = daprClient;
        _logger = logger;
    }

    public async Task RegisterPaymentMethodAsync(RegisterPaymentMethodRequest request)
    {
        _logger.LogInformation("Registering new payment method");
        await _daprClient.InvokeMethodAsync("payments", "paymentmethods", request);
    }
}