using RecommendCoffee.Shipping.Domain.Common;

namespace RecommendCoffee.Shipping.Domain.Aggregates.ShippingOrderAggregate.Events;

public record ShippingOrderCreatedEvent(ShippingOrder Order) : IDomainEvent;
