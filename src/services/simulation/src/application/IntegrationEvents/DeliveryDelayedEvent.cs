using TastyBeans.Shared.Domain;

namespace TastyBeans.Simulation.Application.IntegrationEvents;

[Topic("transport.shipment.delivery-delayed.v1")]
public record DeliveryDelayedEvent(Guid ShippingOrderId);