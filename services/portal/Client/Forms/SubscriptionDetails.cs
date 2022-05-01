using System.ComponentModel.DataAnnotations;

namespace TastyBeans.Portal.Client.Forms;

public record SubscriptionDetails
{
    [Required(ErrorMessage = "Please choose how often you want to receive coffee")]
    public ShippingFrequency? ShippingFrequency { get; set; }
    
    [Required(ErrorMessage = "Please choose the type of subscription you want")]
    public SubscriptionKind? Kind { get; set; }
}
