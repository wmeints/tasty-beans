using TastyBeans.Shared.Domain;

namespace TastyBeans.Simulation.Application.IntegrationEvents;

[Topic("transport.shipment.lost.v1")]
public record ShipmentLostEvent(Guid ShippingOrderId);
