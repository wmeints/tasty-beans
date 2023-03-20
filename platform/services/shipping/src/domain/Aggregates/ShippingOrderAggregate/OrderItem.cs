namespace TastyBeans.Shipping.Domain.Aggregates.ShippingOrderAggregate;

public record OrderItem(Guid ProductId, int Amount);