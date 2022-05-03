using TastyBeans.Shipping.Domain.Aggregates.ShippingOrderAggregate;

namespace TastyBeans.Simulation.Domain.Services.ShippingInformation;

public class ShippingOrder
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public DateTime OrderDate { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; }
}