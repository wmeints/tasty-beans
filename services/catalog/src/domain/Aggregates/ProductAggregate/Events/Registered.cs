using TastyBeans.Shared.Domain;

namespace TastyBeans.Catalog.Domain.Aggregates.ProductAggregate.Events;

[Topic("catalog.product.registered.v1")]
public record Registered(
    Guid ProductId,
    string Name,
    string Description): IDomainEvent;