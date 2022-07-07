namespace TastyBeans.Simulation.Domain.Aggregates.CustomerAggregate.Commands;

public record DeliveryDelayed(Guid ShippingOrderId);