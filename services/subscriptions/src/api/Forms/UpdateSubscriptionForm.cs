using TastyBeans.Subscriptions.Domain.Aggregates.SubscriptionAggregate;

namespace TastyBeans.Subscriptions.Api.Forms;

public class UpdateSubscriptionForm
{
    public ShippingFrequency Frequency { get; set; }
}