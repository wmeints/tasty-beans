namespace RecommendCoffee.Subscriptions.Infrastructure.Agents.Shipping;

public record CreateShippingOrderRequest(Guid CustomerId, IEnumerable<OrderItem> OrderItems);