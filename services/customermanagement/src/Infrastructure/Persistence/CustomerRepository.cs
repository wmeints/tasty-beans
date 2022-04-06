using Microsoft.EntityFrameworkCore;
using RecommendCoffee.CustomerManagement.Domain.Aggregates.CustomerAggregate;
using RecommendCoffee.CustomerManagement.Domain.Common;

namespace RecommendCoffee.CustomerManagement.Infrastructure.Persistence;

public class CustomerRepository : ICustomerRepository
{
    private readonly ApplicationDbContext _applicationDbContext;

    public CustomerRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<PagedResult<Customer>> FindAllAsync(int pageIndex, int pageSize)
    {
        var totalItemCount = await _applicationDbContext.Customers.LongCountAsync();

        var records = await _applicationDbContext.Customers
            .Include(x => x.InvoiceAddress)
            .Include(x => x.ShippingAddress)
            .OrderBy(x => x.LastName)
            .Skip(pageIndex * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<Customer>(records, pageIndex, pageSize, totalItemCount);
    }

    public async Task<Customer?> FindById(Guid id)
    {
        return await _applicationDbContext.Customers
            .Include(x => x.InvoiceAddress)
            .Include(x => x.ShippingAddress)
            .SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<int> InsertAsync(Customer customer)
    {
        await _applicationDbContext.AddAsync(customer);
        return await _applicationDbContext.SaveChangesAsync();
    }

    public async Task<int> UpdateAsync(Customer customer)
    {
        _applicationDbContext.Update(customer);
        return await _applicationDbContext.SaveChangesAsync();
    }
}
