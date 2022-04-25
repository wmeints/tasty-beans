using RecommendCoffee.Registration.Domain.Customers;
using RecommendCoffee.Registration.Domain.Payments;
using RecommendCoffee.Registration.Domain.Subscriptions;

namespace RecommendCoffee.Registration.Api.Forms;

public record StartRegistrationForm(CustomerDetails CustomerDetails, SubscriptionDetails Subscription, PaymentMethodDetails PaymentMethod);