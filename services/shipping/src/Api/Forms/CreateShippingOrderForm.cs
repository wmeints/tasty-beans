using RecommendCoffee.Shipping.Domain.Aggregates.ShippingOrderAggregate;

namespace RecommendCoffee.Shipping.Api.Forms;

public class CreateShippingOrderForm
{
    public Guid CustomerId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Street { get; set; }
    public string HouseNumber { get; set; }
    public string PostalCode { get; set; }
    public string City { get; set; }
    public string CountryCode { get; set; }
    public IEnumerable<OrderItem> OrderItems { get; set; }
}