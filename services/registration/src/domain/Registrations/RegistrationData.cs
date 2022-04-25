using System.Diagnostics.CodeAnalysis;
using RecommendCoffee.Registration.Domain.Customers;
using RecommendCoffee.Registration.Domain.Payments;
using RecommendCoffee.Registration.Domain.Subscriptions;

namespace RecommendCoffee.Registration.Domain.Registrations;

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