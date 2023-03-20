using TastyBeans.Shared.Domain;

namespace TastyBeans.Transport.Domain.Aggregates.ShipmentAggregate.Events;

[Topic("transport.shipment.delivery-attempt-failed.v1")]
public record DeliveryAttemptFailedEvent(Guid ShippingOrderId) : IDomainEvent;