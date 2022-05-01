using TastyBeans.Shared.Domain;

namespace TastyBeans.Shipping.Domain.Aggregates.ShippingOrderAggregate.Events;

[Topic("shipping.shippingorder.created.v1")]
public record ShippingOrderCreatedEvent(ShippingOrder Order) : IDomainEvent;
