using TastyBeans.Shared.Domain;

namespace TastyBeans.Transport.Domain.Aggregates.ShipmentAggregate.Events;

[Topic("transport.shipment.delivery-delayed.v1")]
public record DeliveryDelayedEvent(Guid ShippingOrderId) : IDomainEvent;