using TastyBeans.Shared.Domain;

namespace TastyBeans.Catalog.Domain.Aggregates.ProductAggregate.Events;

[Topic("catalog.product.discontinued.v1")]
public record ProductDiscontinuedEvent(Guid ProductId) : IDomainEvent;
