using Microsoft.EntityFrameworkCore;
using RecommendCoffee.Catalog.Domain.Projections.ProductInfoProjection;
using RecommendCoffee.Catalog.Domain.Projections.ProductInfoProjection.Queries;

namespace RecommendCoffee.Catalog.Infrastructure.Persistence;

public class ProductInformationRepository : IProductInformationRepository
{
    private readonly ApplicationDbContext _applicationDbContext;

    public ProductInformationRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<FindAllProductsQueryResult> FindAll(FindAllProductsQuery query)
    {
        var records = await _applicationDbContext.Products
            .OrderBy(x => x.Name)
            .Skip(query.PageIndex * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync();

        var totalItemCount = await _applicationDbContext.Products.LongCountAsync();

        return new FindAllProductsQueryResult(records,query.PageIndex, query.PageSize, totalItemCount);
    }

    public async Task<FindProductQueryResult> FindById(FindProductQuery query)
    {
        var result = await _applicationDbContext.Products.SingleOrDefaultAsync(x => x.Id == query.ProductId);
        return new FindProductQueryResult(result);
    }

    public async Task Insert(ProductInformation productInformation)
    {
        await _applicationDbContext.Products.AddAsync(productInformation);
        await _applicationDbContext.SaveChangesAsync();
    }
}
