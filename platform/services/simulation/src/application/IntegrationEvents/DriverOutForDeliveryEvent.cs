using TastyBeans.Shared.Domain;

namespace TastyBeans.Simulation.Application.IntegrationEvents;

[Topic("transport.shipment.driver-out-for-delivery.v1")]
public record DriverOutForDeliveryEvent(Guid ShippingOrderId);