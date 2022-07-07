using TastyBeans.Registration.Domain.Customers;
using TastyBeans.Registration.Domain.Payments;
using TastyBeans.Registration.Domain.Subscriptions;

namespace TastyBeans.Registration.Api.Forms;

public record StartRegistrationForm(CustomerDetails CustomerDetails, SubscriptionDetails Subscription, PaymentMethodDetails PaymentMethod);