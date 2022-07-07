using Microsoft.EntityFrameworkCore;
using TastyBeans.Ratings.Domain.Aggregates.ProductAggregate;

namespace TastyBeans.Ratings.Infrastructure.Persistence;

public class ProductRepository: IProductRepository
{
    private readonly ApplicationDbContext _applicationDbContext;

    public ProductRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<int> InsertAsync(Product product)
    {
        using var activity = Activities.ExecuteDatabaseCommand();
        await _applicationDbContext.Products.AddAsync(product);
        return await _applicationDbContext.SaveChangesAsync();
    }

    public async Task<int> UpdateAsync(Product product)
    {
        using var activity = Activities.ExecuteDatabaseCommand();
        _applicationDbContext.Update(product);
        return await _applicationDbContext.SaveChangesAsync();
    }

    public async Task<Product?> FindByIdAsync(Guid id)
    {
        using var activity = Activities.ExecuteDatabaseCommand();
        return await _applicationDbContext.Products.SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<bool> ExistsAsync(Guid productId)
    {
        using var activity = Activities.ExecuteDatabaseCommand();
        return await _applicationDbContext.Products.AnyAsync(x => x.Id == productId);
    }
}