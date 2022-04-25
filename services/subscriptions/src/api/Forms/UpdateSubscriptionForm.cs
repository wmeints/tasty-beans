using RecommendCoffee.Subscriptions.Domain.Aggregates.SubscriptionAggregate;

namespace RecommendCoffee.Subscriptions.Api.Forms;

public class UpdateSubscriptionForm
{
    public ShippingFrequency Frequency { get; set; }
}