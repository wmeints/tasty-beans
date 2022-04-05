using Microsoft.EntityFrameworkCore;
using RecommendCoffee.Ratings.Domain.Aggregates.CustomerAggregate;

namespace RecommendCoffee.Ratings.Infrastructure.Persistence;

public class CustomerRepository: ICustomerRepository
{
    private readonly ApplicationDbContext _applicationDbContext;

    public CustomerRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<int> InsertAsync(Customer customer)
    {
        await _applicationDbContext.Customers.AddAsync(customer);
        return await _applicationDbContext.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _applicationDbContext.Customers.AnyAsync(x => x.Id == id);
    }
}