using TastyBeans.Subscriptions.Domain.Aggregates.SubscriptionAggregate;

namespace TastyBeans.Subscriptions.Api.Forms;

public class CreateSubscriptionForm
{
    public Guid CustomerId { get; set; }
    public ShippingFrequency Frequency { get; set; }
    public SubscriptionKind Kind { get; set; }
}