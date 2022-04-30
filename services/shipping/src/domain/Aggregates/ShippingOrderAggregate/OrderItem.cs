namespace RecommendCoffee.Shipping.Domain.Aggregates.ShippingOrderAggregate;

public record OrderItem(Guid ProductId, int Amount);