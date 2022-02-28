using RecommendCoffee.Subscriptions.Domain.Aggregates.SubscriptionAggregate;

namespace RecommendCoffee.Subscriptions.Api.Forms;

public class CreateSubscriptionForm
{
    public Guid CustomerId { get; set; }
    public ShippingFrequency Frequency { get; set; }
    public SubscriptionKind Kind { get; set; }
}