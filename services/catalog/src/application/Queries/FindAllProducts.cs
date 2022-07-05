using MediatR;
using TastyBeans.Catalog.Domain.Aggregates.ProductAggregate;
using TastyBeans.Shared.Domain;

namespace TastyBeans.Catalog.Application.Queries;

public record FindAllProducts(int PageIndex, int PageSize): IRequest<PagedResult<Product>>;
