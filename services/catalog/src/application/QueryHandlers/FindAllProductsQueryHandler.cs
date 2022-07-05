using MediatR;
using TastyBeans.Catalog.Application.Queries;
using TastyBeans.Catalog.Domain.Aggregates.ProductAggregate;
using TastyBeans.Shared.Domain;

namespace TastyBeans.Catalog.Application.QueryHandlers;

public class FindAllProductsQueryHandler : IRequestHandler<FindAllProducts, PagedResult<Product>>
{
    private readonly IProductRepository _productRepository;

    public FindAllProductsQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<PagedResult<Product>> Handle(FindAllProducts request,
        CancellationToken cancellationToken = default)
    {
        return await _productRepository.FindAllAsync(request.PageIndex, request.PageSize);
    }
}