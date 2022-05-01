using TastyBeans.Shared.Domain;

namespace TastyBeans.Transport.Domain.Aggregates.ShipmentAggregate.Events;

[Topic("transport.shipment.sent.v1")]
public record ShipmentSentEvent(Guid ShippingOrderId) : IDomainEvent;