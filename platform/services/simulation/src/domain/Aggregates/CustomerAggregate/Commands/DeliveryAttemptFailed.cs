namespace TastyBeans.Simulation.Domain.Aggregates.CustomerAggregate.Commands;

public record DeliveryAttemptFailed(Guid ShippingOrderId);