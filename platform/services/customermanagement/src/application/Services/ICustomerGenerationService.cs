namespace TastyBeans.CustomerManagement.Application.Services;

public interface ICustomerGenerationService
{
    Task GenerateAsync(string filename);
}