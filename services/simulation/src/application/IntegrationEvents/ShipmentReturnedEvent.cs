using TastyBeans.Shared.Domain;

namespace TastyBeans.Simulation.Application.IntegrationEvents;

[Topic("transport.shipment.returned.v1")]
public record ShipmentReturnedEvent(Guid ShippingOrderId);