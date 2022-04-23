namespace RecommendCoffee.Shipping.Domain.Aggregates.ShippingOrderAggregate.Commands;

public record CreateShippingOrderCommand(Guid CustomerId, IEnumerable<OrderItem> OrderItems);