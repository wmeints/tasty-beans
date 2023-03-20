using TastyBeans.Registration.Domain.Customers;
using TastyBeans.Registration.Domain.Payments;
using TastyBeans.Registration.Domain.Subscriptions;

namespace TastyBeans.Registration.Domain.Registrations.Commands;

public record StartRegistrationCommand(
    Guid CustomerId, 
    CustomerDetails CustomerDetails, 
    SubscriptionDetails SubscriptionDetails,
    PaymentMethodDetails PaymentMethodDetails);