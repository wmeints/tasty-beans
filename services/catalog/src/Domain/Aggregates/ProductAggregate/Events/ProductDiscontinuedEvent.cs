using RecommendCoffee.Catalog.Domain.Common;

namespace RecommendCoffee.Catalog.Domain.Aggregates.ProductAggregate.Events;

[Topic("catalog.product.discontinued.v1")]
public record ProductDiscontinuedEvent(Guid ProductId) : IDomainEvent;
