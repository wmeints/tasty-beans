using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using RecommendCoffee.Catalog.Domain.Aggregates.ProductAggregate;
using RecommendCoffee.Catalog.Domain.Common;

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
        return await _applicationDbContext.Products
            .Include(x => x.Variants)
            .SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<int> InsertAsync(Product product)
    {
        await _applicationDbContext.Products.AddAsync(product);
        return await _applicationDbContext.SaveChangesAsync();
    }

    public async Task<int> UpdateAsync(Product product)
    {
        _applicationDbContext.Update(product);
        return await _applicationDbContext.SaveChangesAsync();
    }

    public async Task<int> DeleteAsync(Product product)
    {
        _applicationDbContext.Remove(product);
        return await _applicationDbContext.SaveChangesAsync();
    }
}