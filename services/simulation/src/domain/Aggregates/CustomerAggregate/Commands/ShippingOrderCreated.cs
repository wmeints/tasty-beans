namespace TastyBeans.Simulation.Domain.Aggregates.CustomerAggregate.Commands;

public record ShippingOrderCreated(Guid CustomerId, Guid ShippingOrderId);