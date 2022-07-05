using TastyBeans.Catalog.Domain.Aggregates.ProductAggregate;

namespace TastyBeans.Catalog.Application.Queries;

public record FindProductByIdResult(Product? Product);
