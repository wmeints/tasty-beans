using TastyBeans.Shared.Domain;

namespace TastyBeans.Simulation.Application.IntegrationEvents;

[Topic("transport.shipment.delivered.v1")]
public record ShipmentDeliveredEvent(Guid ShippingOrderId);
