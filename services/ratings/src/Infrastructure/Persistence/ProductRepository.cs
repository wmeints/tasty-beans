using Microsoft.EntityFrameworkCore;
using RecommendCoffee.Ratings.Domain.Aggregates.ProductAggregate;

namespace RecommendCoffee.Ratings.Infrastructure.Persistence;

public class ProductRepository: IProductRepository
{
    private readonly ApplicationDbContext _applicationDbContext;

    public ProductRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
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

    public async Task<Product?> FindByIdAsync(Guid id)
    {
        return await _applicationDbContext.Products.SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<bool> ExistsAsync(Guid productId)
    {
        return await _applicationDbContext.Products.AnyAsync(x => x.Id == productId);
    }
}