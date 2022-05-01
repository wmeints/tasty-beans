namespace TastyBeans.Registration.Domain.Customers;

public record CustomerDetails(
    string FirstName, 
    string LastName, 
    string EmailAddress, 
    string TelephoneNumber, 
    Address InvoiceAddress, 
    Address ShippingAddress);