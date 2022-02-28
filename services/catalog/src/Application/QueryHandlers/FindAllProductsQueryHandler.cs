using RecommendCoffee.Catalog.Domain.Aggregates.ProductAggregate;
using RecommendCoffee.Catalog.Domain.Common;

namespace RecommendCoffee.Catalog.Application.QueryHandlers;

public class FindAllProductsQueryHandler
{
    private readonly IProductRepository _productRepository;

    public FindAllProductsQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<PagedResult<Product>> ExecuteAsync(int pageIndex, int pageSize)
    {
        return await _productRepository.FindAllAsync(pageIndex, pageSize);
    }
}