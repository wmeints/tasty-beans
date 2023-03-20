using CsvHelper.Configuration;
using TastyBeans.CustomerManagement.Application.Services;

namespace TastyBeans.CustomerManagement.Infrastructure.Persistence;

public class CustomerSampleMap: ClassMap<CustomerSample>
{
    public CustomerSampleMap()
    {
        Map(x => x.Id).Index(0);
        Map(x => x.FirstName).Index(1);
        Map(x => x.LastName).Index(2);
    }
}