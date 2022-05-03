namespace TastyBeans.Simulation.Application.Services.Registration;

public record CustomerDetails(
    string FirstName, 
    string LastName, 
    string EmailAddress, 
    string TelephoneNumber, 
    Address InvoiceAddress, 
    Address ShippingAddress);