using TastyBeans.CustomerManagement.Domain.Aggregates.CustomerAggregate;
using TastyBeans.Shared.Domain;

namespace TastyBeans.CustomerManagement.Application.QueryHandlers;

public class FindAllCustomersQueryHandler
{
    private readonly ICustomerRepository _customerRepository;

    public FindAllCustomersQueryHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<PagedResult<Customer>> ExecuteAsync(int pageIndex, int pageSize)
    {
        using var activity = Activities.ExecuteQuery("FindAllCustomers");
        return await _customerRepository.FindAllAsync(pageIndex, pageSize);
    }
}
