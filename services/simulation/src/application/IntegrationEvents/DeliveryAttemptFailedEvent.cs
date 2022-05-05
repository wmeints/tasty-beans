using TastyBeans.Shared.Domain;

namespace TastyBeans.Simulation.Application.IntegrationEvents;

[Topic("transport.shipment.delivery-attempt-failed.v1")]
public record DeliveryAttemptFailedEvent(Guid ShippingOrderId);