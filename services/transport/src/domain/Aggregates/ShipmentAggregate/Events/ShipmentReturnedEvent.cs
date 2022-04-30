using RecommendCoffee.Shared.Domain;

namespace RecommendCoffee.Transport.Domain.Aggregates.ShipmentAggregate.Events;

[Topic("transport.shipment.returned.v1")]
public record ShipmentReturnedEvent(Guid ShippingOrderId) : IDomainEvent;