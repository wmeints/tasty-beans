using RecommendCoffee.CustomerManagement.Domain.Aggregates.CustomerAggregate;

namespace RecommendCoffee.CustomerManagement.Api.Forms
{
    public class RegisterCustomerForm
    {
        public Guid CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Address InvoiceAddress { get; set; }
        public Address ShippingAddress { get; set; }
        public string TelephoneNumber { get; set; }
        public string EmailAddress { get; set; }
    }
}
