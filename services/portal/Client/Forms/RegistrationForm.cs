using System.ComponentModel.DataAnnotations;

namespace RecommendCoffee.Portal.Client.Forms;

public class StartRegistrationForm
{
    [ValidateComplexType]
    public CustomerDetails CustomerDetails { get; set; } = new CustomerDetails();
    
    [ValidateComplexType]
    public SubscriptionDetails Subscription { get; set; } = new SubscriptionDetails();
    
    [ValidateComplexType]
    public PaymentMethodDetails PaymentMethod { get; set; } = new PaymentMethodDetails();
}