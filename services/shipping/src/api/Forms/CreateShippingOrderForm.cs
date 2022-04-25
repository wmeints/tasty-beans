using RecommendCoffee.Shipping.Domain.Aggregates.ShippingOrderAggregate;

namespace RecommendCoffee.Shipping.Api.Forms;

public class CreateShippingOrderForm
{
    public Guid CustomerId { get; set; }
    public IEnumerable<OrderItem> OrderItems { get; set; }
}