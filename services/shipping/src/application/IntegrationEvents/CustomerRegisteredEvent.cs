namespace RecommendCoffee.Shipping.Application.IntegrationEvents;

public record CustomerRegisteredEvent(
    Guid CustomerId, 
    string FirstName, 
    string LastName, 
    Address InvoiceAddress, 
    Address ShippingAddress, 
    string EmailAddress, 
    string TelephoneNumber
);