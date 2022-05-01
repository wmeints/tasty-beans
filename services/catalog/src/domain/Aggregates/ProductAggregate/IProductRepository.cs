using TastyBeans.Shared.Domain;

namespace TastyBeans.Catalog.Domain.Aggregates.ProductAggregate;

public interface IProductRepository
{
    Task<PagedResult<Product>> FindAllAsync(int pageIndex, int pageSize);
    Task<Product?> FindByIdAsync(Guid id);
    Task<int> InsertAsync(Product product);
    Task<int> UpdateAsync(Product product);
    Task<int> DeleteAsync(Product product);
}