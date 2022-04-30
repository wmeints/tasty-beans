using RecommendCoffee.Shared.Domain;

namespace RecommendCoffee.Transport.Domain.Aggregates.ShipmentAggregate.Events;

[Topic("transport.shipment.driver-out-for-delivery.v1")]
public record DriverOutForDeliveryEvent(Guid ShippingOrderId) : IDomainEvent;