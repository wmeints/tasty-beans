namespace RecommendCoffee.Shipping.Domain.Aggregates.ShippingOrderAggregate;

public record OrderItem(Guid ProductId, string Description, int Amount);