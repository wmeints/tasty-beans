using Microsoft.EntityFrameworkCore;
using RecommendCoffee.Catalog.Domain.Aggregates.ProductAggregate;
using RecommendCoffee.Shared.Domain;

namespace RecommendCoffee.Catalog.Infrastructure.Persistence;

public class ProductRepository: IProductRepository
{
    private readonly ApplicationDbContext _applicationDbContext;

    public ProductRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<PagedResult<Product>> FindAllAsync(int pageIndex, int pageSize)
    {
        using var activity = Activities.ExecuteDatabaseCommand();
        var items = await _applicationDbContext.Products
            .Include(x => x.Variants)
            .OrderBy(x => x.Name)
            .Skip(pageIndex * pageSize)
            .Take(pageSize)
            .ToListAsync();
        
        var totalItems = await _applicationDbContext.Products.LongCountAsync();

        return new PagedResult<Product>(items, pageIndex, pageSize, totalItems);
    }

    public async Task<Product?> FindByIdAsync(Guid id)
    {
        using var activity = Activities.ExecuteDatabaseCommand();
        
        return await _applicationDbContext.Products
            .Include(x => x.Variants)
            .SingleOrDefaultAsync(x => x.Id == id);
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

    public async Task<int> DeleteAsync(Product product)
    {
        using var activity = Activities.ExecuteDatabaseCommand();
        
        _applicationDbContext.Remove(product);
        return await _applicationDbContext.SaveChangesAsync();
    }
}