namespace RecommendCoffee.Shipping.Domain.Aggregates.ShippingOrderAggregate;

public class ShippingOrder
{
    public Guid Id { get; private set; }
    public Guid CustomerId { get; private set; }
}