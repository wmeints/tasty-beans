using TastyBeans.Shared.Domain;

namespace TastyBeans.Transport.Domain.Aggregates.ShipmentAggregate.Events;

[Topic("transport.shipment.sorted.v1")]
public record ShipmentSortedEvent(Guid ShippingOrderId) : IDomainEvent;