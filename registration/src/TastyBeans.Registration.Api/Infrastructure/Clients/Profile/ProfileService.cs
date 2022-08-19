namespace TastyBeans.Registration.Api.Infrastructure.Clients.Profile;

public class ProfileService: IProfileService
{
    private readonly HttpClient _httpClient;

    public ProfileService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task RegisterCustomer(RegisterCustomerRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("/profiles", request);
        response.EnsureSuccessStatusCode();
    }
}