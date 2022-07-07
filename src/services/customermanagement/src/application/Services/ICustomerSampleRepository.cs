namespace TastyBeans.CustomerManagement.Application.Services;

public interface ICustomerSampleRepository
{
    Task<IEnumerable<CustomerSample>> GetCustomerSamples(string filename);
}