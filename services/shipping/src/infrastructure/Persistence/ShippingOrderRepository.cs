using Microsoft.EntityFrameworkCore;
using TastyBeans.Shared.Domain;
using TastyBeans.Shipping.Domain.Aggregates.ShippingOrderAggregate;

namespace TastyBeans.Shipping.Infrastructure.Persistence;

public class ShippingOrderRepository: IShippingOrderRepository
{
    private readonly ApplicationDbContext _applicationDbContext;

    public ShippingOrderRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<ShippingOrder?> FindByIdAsync(Guid id)
    {
        using var activity = Activities.ExecuteDatabaseCommand();
        return await _applicationDbContext.ShippingOrders.SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<PagedResult<ShippingOrder>> FindAllAsync(int pageIndex, int pageSize)
    {
        using var activity = Activities.ExecuteDatabaseCommand();
        
        var items = await _applicationDbContext.ShippingOrders
            .Include(x=>x.OrderItems)
            .OrderBy(x => x.OrderDate)
            .Skip(pageIndex * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var totalItemCount = await _applicationDbContext.ShippingOrders.LongCountAsync();

        return new PagedResult<ShippingOrder>(items, pageIndex, pageSize, totalItemCount);
    }

    public async Task<int> InsertAsync(ShippingOrder shippingOrder)
    {
        using var activity = Activities.ExecuteDatabaseCommand();

        await _applicationDbContext.AddAsync(shippingOrder);
        return await _applicationDbContext.SaveChangesAsync();
    }

    public async Task<int> UpdateAsync(ShippingOrder shippingOrder)
    {
        using var activity = Activities.ExecuteDatabaseCommand();

        _applicationDbContext.Update(shippingOrder);
        return await _applicationDbContext.SaveChangesAsync();
    }

    public async Task<int> DeleteAsync(ShippingOrder shippingOrder)
    {
        _applicationDbContext.Remove(shippingOrder);
        return await _applicationDbContext.SaveChangesAsync();
    }
}