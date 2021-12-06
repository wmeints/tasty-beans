using Microsoft.EntityFrameworkCore;
using RecommendCoffee.Customers.Domain.Projections.CustomerInfoProjection;
using RecommendCoffee.Customers.Domain.Projections.CustomerInfoProjection.Queries;

namespace RecommendCoffee.Customers.Infrastructure.Persistence;

public class CustomerInformationRepository: ICustomerInformationRepository
{
    private readonly ApplicationDbContext _applicationDbContext;

    public CustomerInformationRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task Insert(CustomerInformation customerInformation)
    {
        await _applicationDbContext.Customers.AddAsync(customerInformation);
        await _applicationDbContext.SaveChangesAsync();
    }

    public async Task<FindCustomerQueryResult> FindById(FindCustomerQuery query)
    {
        var customer = await _applicationDbContext.Customers
            .Include(x=>x.ShippingAddress)
            .Include(x=>x.InvoiceAddress)
            .SingleOrDefaultAsync(x => x.Id == query.CustomerId);

        return new FindCustomerQueryResult(customer);
    }

    public async Task<FindAllCustomersQueryResult> FindAll(FindAllCustomersQuery query)
    {
        var items = await _applicationDbContext.Customers
            .Include(x=>x.ShippingAddress)
            .Include(x=>x.InvoiceAddress)
            .OrderBy(x => x.LastName)
            .ThenBy(x=>x.FirstName)
            .Skip(query.PageIndex * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync();

        var totalRecordCount = await _applicationDbContext.Customers.LongCountAsync();

        return new FindAllCustomersQueryResult(items, query.PageSize, query.PageSize, totalRecordCount);
    }
}