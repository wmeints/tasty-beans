namespace TastyBeans.Catalog.Domain.Aggregates.ProductAggregate.Commands;

public record Register(Guid Id, string Name, string Description);