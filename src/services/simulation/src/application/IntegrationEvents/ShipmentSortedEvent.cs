using TastyBeans.Shared.Domain;

namespace TastyBeans.Simulation.Application.IntegrationEvents;

[Topic("transport.shipment.sorted.v1")]
public record ShipmentSortedEvent(Guid ShippingOrderId);