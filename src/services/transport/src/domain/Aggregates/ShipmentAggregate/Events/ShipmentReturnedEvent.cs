using TastyBeans.Shared.Domain;

namespace TastyBeans.Transport.Domain.Aggregates.ShipmentAggregate.Events;

[Topic("transport.shipment.returned.v1")]
public record ShipmentReturnedEvent(Guid ShippingOrderId) : IDomainEvent;