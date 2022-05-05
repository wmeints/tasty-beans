namespace TastyBeans.Simulation.Application.Services.Registration;

public class RegistrationRequest
{
    public CustomerDetails CustomerDetails { get; set; }
    public SubscriptionDetails Subscription { get; set; } 
    public PaymentMethodDetails PaymentMethod { get; set; }
}