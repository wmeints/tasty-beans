using TastyBeans.Shipping.Domain.Aggregates.ShippingOrderAggregate;

namespace TastyBeans.Shipping.Api.Forms;

public class CreateShippingOrderForm
{
    public Guid CustomerId { get; set; }
    public IEnumerable<OrderItem> OrderItems { get; set; }
}