using RecommendCoffee.Customers.Domain.Aggregates.CustomerAggregate.Commands;

namespace RecommendCoffee.Customers.Api.Models.Requests
{
    public record RegisterCustomerRequest(string FirstName, string LastName, AddressDto InvoiceAddress, AddressDto ShippingAddress)
    {
        public RegisterCustomerCommand ToCommand()
        {
            return new RegisterCustomerCommand(
                FirstName,
                LastName,
                new Address(InvoiceAddress.StreetName, InvoiceAddress.HouseNumber, InvoiceAddress.ZipCode, InvoiceAddress.City),
                new Address(ShippingAddress.StreetName, ShippingAddress.HouseNumber, ShippingAddress.ZipCode, ShippingAddress.City)
            );
        }
    }
}
