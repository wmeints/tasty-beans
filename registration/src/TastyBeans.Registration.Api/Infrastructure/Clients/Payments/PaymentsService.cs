namespace TastyBeans.Registration.Api.Infrastructure.Clients.Payments;

public class PaymentsService : IPaymentsService
{
    private readonly HttpClient _httpClient;

    public PaymentsService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task RegisterPaymentMethod(RegisterPaymentMethodRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("/payment-methods", request);
        response.EnsureSuccessStatusCode();
    }
}