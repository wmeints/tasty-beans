using System.Diagnostics.CodeAnalysis;
using TastyBeans.Registration.Domain.Customers;
using TastyBeans.Registration.Domain.Payments;
using TastyBeans.Registration.Domain.Subscriptions;

namespace TastyBeans.Registration.Domain.Registrations;

public class RegistrationData
{
    public RegistrationState State { get; set; }
    
    public Guid CustomerId { get; set; }
    
    [NotNull]
    public CustomerDetails? CustomerDetails { get; set; }
    
    [NotNull]
    public SubscriptionDetails? SubscriptionDetails { get; set; }
    
    [NotNull]
    public PaymentMethodDetails? PaymentMethodDetails { get; set; }
}