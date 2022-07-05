using TastyBeans.Catalog.Domain.Aggregates.ProductAggregate;

namespace TastyBeans.Catalog.Application.Commands;

public record RegisterProductCommand(string Name, string Description);