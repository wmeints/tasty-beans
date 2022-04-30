using RecommendCoffee.Shared.Domain;

namespace RecommendCoffee.Transport.Domain.Aggregates.ShipmentAggregate.Events;

[Topic("transport.shipment.delivery-attempt-failed.v1")]
public record DeliveryAttemptFailedEvent() : IDomainEvent;