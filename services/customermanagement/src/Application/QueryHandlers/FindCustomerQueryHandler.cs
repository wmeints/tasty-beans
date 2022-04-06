using RecommendCoffee.CustomerManagement.Domain.Aggregates.CustomerAggregate;

namespace RecommendCoffee.CustomerManagement.Application.QueryHandlers;

public class FindCustomerQueryHandler
{
    private readonly ICustomerRepository _customerRepository;

    public FindCustomerQueryHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<Customer?> ExecuteAsync(Guid customerId)
    {
        return await _customerRepository.FindById(customerId);
    }
}
