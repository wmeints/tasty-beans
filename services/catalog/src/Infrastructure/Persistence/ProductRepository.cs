using RecommendCoffee.Catalog.Domain.Aggregates.ProductAggregate;
using RecommendCoffee.Catalog.Domain.Common;

namespace Infrastructure.Persistence;

public class ProductRepository: IProductRepository
{
    public Task<PagedResult<Product>> FindAllAsync(int pageIndex, int pageSize)
    {
        throw new NotImplementedException();
    }

    public Task<Product?> FindByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<int> InsertAsync(Product product)
    {
        throw new NotImplementedException();
    }

    public Task<int> UpdateAsync(Product product)
    {
        throw new NotImplementedException();
    }

    public Task<int> DeleteAsync(Product product)
    {
        throw new NotImplementedException();
    }
}