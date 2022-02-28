using RecommendCoffee.Registration.Domain.Customers;
using RecommendCoffee.Registration.Domain.Payments;
using RecommendCoffee.Registration.Domain.Subscriptions;

namespace RecommendCoffee.Registration.Domain.Registrations.Commands;

public record StartRegistrationCommand(
    Guid CustomerId, 
    CustomerDetails CustomerDetails, 
    SubscriptionDetails SubscriptionDetails,
    PaymentMethodDetails PaymentMethodDetails);