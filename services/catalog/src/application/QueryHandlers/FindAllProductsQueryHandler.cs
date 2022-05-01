using TastyBeans.Catalog.Domain.Aggregates.ProductAggregate;
using TastyBeans.Shared.Domain;

namespace TastyBeans.Catalog.Application.QueryHandlers;

public class FindAllProductsQueryHandler
{
    private readonly IProductRepository _productRepository;

    public FindAllProductsQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<PagedResult<Product>> ExecuteAsync(int pageIndex, int pageSize)
    {
        using var activity = Activities.ExecuteQuery("FindAllProducts");
        return await _productRepository.FindAllAsync(pageIndex, pageSize);
    }
}