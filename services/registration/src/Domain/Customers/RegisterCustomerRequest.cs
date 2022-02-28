namespace RecommendCoffee.Registration.Domain.Customers;

public record RegisterCustomerRequest(
    Guid CustomerId, 
    string FirstName, 
    string LastName, 
    string EmailAddress, 
    string TelephoneNumber, 
    Address InvoiceAddress, 
    Address ShippingAddress);