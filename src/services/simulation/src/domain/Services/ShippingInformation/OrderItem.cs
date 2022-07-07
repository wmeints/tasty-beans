namespace TastyBeans.Shipping.Domain.Aggregates.ShippingOrderAggregate;

public class OrderItem
{
    public Guid ProductId { get; set; }
    public int Amount { get; set; }
}