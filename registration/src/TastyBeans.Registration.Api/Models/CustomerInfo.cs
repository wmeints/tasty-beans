namespace TastyBeans.Registration.Api.Models;

public record CustomerInfo(
    string FirstName, 
    string LastName,
    string EmailAddress,
    Address ShippingAddress,
    Address InvoiceAddress);