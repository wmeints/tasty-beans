using RecommendCoffee.Catalog.Domain.Projections.ProductInfoProjection.Queries;

namespace RecommendCoffee.Catalog.Domain.Projections.ProductInfoProjection;

public interface IProductInformationRepository
{
    Task Insert(ProductInformation productInformation);
    Task<FindAllProductsQueryResult> FindAll(FindAllProductsQuery query);
    Task<FindProductQueryResult> FindById(FindProductQuery query);
}
