using TastyBeans.Shared.Domain;

namespace TastyBeans.Transport.Domain.Aggregates.ShipmentAggregate.Events;

[Topic("transport.shipment.lost.v1")]
public record ShipmentLostEvent(Guid ShippingOrderId) : IDomainEvent;
