using Microsoft.EntityFrameworkCore;
using TastyBeans.Recommendations.Domain.Aggregates.CustomerAggregate;

namespace TastyBeans.Recommendations.Infrastructure.Persistence;

public class CustomerRepository: ICustomerRepository
{
    private readonly ApplicationDbContext _applicationDbContext;

    public CustomerRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<int> InsertAsync(Customer customer)
    {
        using var activity = Activities.ExecuteDatabaseCommand();
        await _applicationDbContext.Customers.AddAsync(customer);
        return await _applicationDbContext.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        using var activity = Activities.ExecuteDatabaseCommand();
        return await _applicationDbContext.Customers.AnyAsync(x => x.Id == id);
    }
}