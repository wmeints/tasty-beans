using RecommendCoffee.Shared.Domain;

namespace RecommendCoffee.Transport.Domain.Aggregates.ShipmentAggregate.Events;

[Topic("transport.shipment.delivered.v1")]
public record ShipmentDeliveredEvent(Guid ShippingOrderId) : IDomainEvent;
