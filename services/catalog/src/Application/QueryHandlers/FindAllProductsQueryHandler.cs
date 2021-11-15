using RecommendCoffee.Catalog.Domain.Projections.ProductInfoProjection;
using RecommendCoffee.Catalog.Domain.Projections.ProductInfoProjection.Queries;

namespace RecommendCoffee.Catalog.Application.QueryHandlers;

public class FindAllProductsQueryHandler: IQueryHandler<FindAllProductsQuery, FindAllProductsQueryResult>
{
    private readonly IProductInformationRepository _productInformationRepository;

    public FindAllProductsQueryHandler(IProductInformationRepository productInformationRepository)
    {
        _productInformationRepository = productInformationRepository;
    }

    public async Task<FindAllProductsQueryResult> Execute(FindAllProductsQuery query)
    {
        return await _productInformationRepository.FindAll(query);
    }
}
