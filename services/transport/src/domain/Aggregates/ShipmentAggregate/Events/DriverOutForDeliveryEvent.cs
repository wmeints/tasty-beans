using TastyBeans.Shared.Domain;

namespace TastyBeans.Transport.Domain.Aggregates.ShipmentAggregate.Events;

[Topic("transport.shipment.driver-out-for-delivery.v1")]
public record DriverOutForDeliveryEvent(Guid ShippingOrderId) : IDomainEvent;