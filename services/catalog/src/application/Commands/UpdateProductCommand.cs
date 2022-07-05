using TastyBeans.Catalog.Domain.Aggregates.ProductAggregate;

namespace TastyBeans.Catalog.Application.Commands;

public record UpdateProductCommand(
    Guid ProductId, 
    string Name, 
    string Description);
