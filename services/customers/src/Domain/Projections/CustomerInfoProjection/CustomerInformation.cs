namespace RecommendCoffee.Customers.Domain.Projections.CustomerInfoProjection;

public class CustomerInformation
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public AddressInformation InvoiceAddress { get; set; }
    public AddressInformation ShippingAddress { get; set; }
}