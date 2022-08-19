namespace TastyBeans.Registration.Api.Infrastructure.Clients.Profile;

public interface IProfileService
{
    Task RegisterCustomer(RegisterCustomerRequest request);
}