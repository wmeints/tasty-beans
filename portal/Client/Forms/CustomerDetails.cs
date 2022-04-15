namespace RecommendCoffee.Portal.Client.Forms;

public record CustomerDetails(
    string FirstName, 
    string LastName, 
    string EmailAddress, 
    string TelephoneNumber, 
    Address InvoiceAddress, 
    Address ShippingAddress);