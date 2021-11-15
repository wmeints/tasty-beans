namespace RecommendCoffee.Catalog.Domain.Projections.ProductInfoProjection.Queries;

public record FindAllProductsQueryResult(IEnumerable<ProductInformation> Items, int PageIndex, int PageSize, long TotalItemCount);
