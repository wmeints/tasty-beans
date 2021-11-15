using RecommendCoffee.Catalog.Domain.Projections.ProductInfoProjection;
using RecommendCoffee.Catalog.Domain.Projections.ProductInfoProjection.Queries;

namespace RecommendCoffee.Catalog.Application.QueryHandlers;

public class FindProductQueryHandler : IQueryHandler<FindProductQuery, FindProductQueryResult>
{
    private readonly IProductInformationRepository _productInformationRepository;

    public FindProductQueryHandler(IProductInformationRepository productInformationRepository)
    {
        _productInformationRepository = productInformationRepository;
    }

    public async Task<FindProductQueryResult> Execute(FindProductQuery query)
    {
        return await _productInformationRepository.FindById(query);
    }
}
