using TastyBeans.Shared.Domain;

namespace TastyBeans.Simulation.Application.IntegrationEvents;

[Topic("shipping.shippingorder.created.v1")]
public record ShippingOrderCreatedEvent(Guid CustomerId, Guid ShippingOrderId);
