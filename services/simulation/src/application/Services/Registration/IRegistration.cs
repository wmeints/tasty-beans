namespace TastyBeans.Simulation.Application.Services.Registration;

public interface IRegistration
{
    Task RegisterCustomerAsync(RegistrationRequest request);
}