using MediatR;
using TastyBeans.Catalog.Application.Queries;
using TastyBeans.Catalog.Domain.Aggregates.ProductAggregate;

namespace TastyBeans.Catalog.Application.QueryHandlers;

public class FindProductByIdQueryHandler : IRequestHandler<FindProductById, FindProductByIdResult>
{
    private readonly IProductRepository _productRepository;

    public FindProductByIdQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<FindProductByIdResult> Handle(FindProductById request,
        CancellationToken cancellationToken = default)
    {
        var result = await _productRepository.FindByIdAsync(request.Id);

        return new FindProductByIdResult(result);
    }
}