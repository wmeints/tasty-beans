namespace RecommendCoffee.Shipping.Domain.Aggregates.ProductAggregate;

public interface IProductRepository
{
    Task<int> InsertAsync(Product product);
    Task<int> UpdateAsync(Product product);
    Task<Product?> FindByIdAsync(Guid id);
    Task<bool> ExistsAsync(Guid productId);
}