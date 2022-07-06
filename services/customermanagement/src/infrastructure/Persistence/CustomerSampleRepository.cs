using System.Globalization;
using CsvHelper;
using TastyBeans.CustomerManagement.Application.Services;

namespace TastyBeans.CustomerManagement.Infrastructure.Persistence;

public class CustomerSampleRepository: ICustomerSampleRepository
{
    public Task<IEnumerable<CustomerSample>> GetCustomerSamples(string filename)
    {
        using var reader = new StreamReader(File.OpenRead(filename));
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        csv.Context.RegisterClassMap<CustomerSampleMap>();
        
        var customerSamples = (IEnumerable<CustomerSample>)csv.GetRecords<CustomerSample>().ToList();
        
        return Task.FromResult(customerSamples);
    }
}