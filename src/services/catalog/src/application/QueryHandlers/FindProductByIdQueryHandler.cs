using TastyBeans.Catalog.Domain.Aggregates.ProductAggregate;

namespace TastyBeans.Catalog.Application.QueryHandlers;

public class FindProductByIdQueryHandler
{
    private readonly IProductRepository _productRepository;

    public FindProductByIdQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Product?> ExecuteAsync(Guid productId)
    {
        using var activity = Activities.ExecuteQuery("FindProductById");
        return await _productRepository.FindByIdAsync(productId);
    }
}