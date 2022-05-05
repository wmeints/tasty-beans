using TastyBeans.Shared.Domain;

namespace TastyBeans.Simulation.Application.IntegrationEvents;

[Topic("transport.shipment.sent.v1")]
public record ShipmentSentEvent(Guid ShippingOrderId);