using System.Diagnostics.CodeAnalysis;
using NodaTime;
using TastyBeans.Profile.Domain.Aggregates.CustomerAggregate.Events;
using AggregateRoot = TastyBeans.Profile.Domain.Shared.AggregateRoot;

namespace TastyBeans.Profile.Domain.Aggregates.CustomerAggregate;

/// <summary>
/// <para>
/// Customers can subscribe to the coffee delivery service by registering a new profile.
/// Once they're registered, they receive coffee once per month.
/// </para>
/// <para>
/// Customers can unsubscribe from the coffee delivery service at any time. The subscription ends at the end of the
/// month. If they don't subscribe again, the coffee delivery service will no longer send them coffee.
/// </para>
/// </summary>
public class Customer : AggregateRoot
{
    [NotNull]
    public string? FirstName { get; private set; } 
    
    [NotNull]
    public string? LastName { get; private set; }
    
    [NotNull]
    public string? EmailAddress { get; private set; }
    
    [NotNull]
    public Address? InvoiceAddress { get; private set; }
    
    [NotNull]
    public Address? ShippingAddress { get; private set; }
    
    public Subscription? Subscription { get; private set; } = null;

    public static Customer Register(Guid customerId, string firstName, string lastName, Address shippingAddress,
        Address invoiceAddress, string emailAddress, DateTime subscriptionStartDate)
    {
        var customer = new Customer();

        customer.Emit(new CustomerRegistered(
            customerId,
            firstName,
            lastName,
            shippingAddress,
            invoiceAddress,
            emailAddress));

        customer.Emit(new CustomerSubscribed(subscriptionStartDate));

        return customer;
    }

    public void Subscribe(DateTime subscriptionStartDate)
    {
        if (Subscription is { EndDate: null })
        {
            throw new InvalidOperationException("Customer is already subscribed");
        }

        Emit(new CustomerSubscribed(subscriptionStartDate));
    }

    public void Unsubscribe(DateTime subscriptionEndDate)
    {
        if (Subscription == null || Subscription.EndDate != null)
        {
            throw new InvalidOperationException("Customer is not subscribed");
        }

        // Subscriptions always end at the end of the current month.
        var localEndDate = LocalDateTime.FromDateTime(subscriptionEndDate);
        var adjustedEndDate = localEndDate.With(DateAdjusters.EndOfMonth);

        Emit(new CustomerUnsubscribed(Id, adjustedEndDate.ToDateTimeUnspecified()));
    }

    public void EndSubscription()
    {
        if (Subscription is { EndDate: null })
        {
            throw new InvalidOperationException("Subscription is active");
        }

        if (Subscription == null)
        {
            throw new InvalidOperationException("Customer is not subscribed");
        }

        Emit(new CustomerSubscriptionEnded(Id, Subscription.StartDate, Subscription.EndDate.Value));
    }

    protected override bool TryApplyDomainEvent(object domainEvent)
    {
        switch (domainEvent)
        {
            case CustomerRegistered customerRegistered:
                Apply(customerRegistered);
                break;
            case CustomerSubscribed customerSubscribed:
                Apply(customerSubscribed);
                break;
            case CustomerUnsubscribed customerUnsubscribed:
                Apply(customerUnsubscribed);
                break;
            case CustomerSubscriptionEnded customerSubscriptionEnded:
                Apply(customerSubscriptionEnded);
                break;
            default:
                return false;
        }

        return true;
    }

    private void Apply(CustomerSubscriptionEnded customerSubscriptionEnded)
    {
        Subscription = null;
        Version++;
    }

    private void Apply(CustomerSubscribed customerSubscribed)
    {
        Subscription = new Subscription(customerSubscribed.SubscriptionStartDate);
        Version++;
    }

    private void Apply(CustomerUnsubscribed customerUnsubscribed)
    {
        if (Subscription != null)
        {
            Subscription = Subscription with { EndDate = customerUnsubscribed.SubscriptionEndDate };
        }

        Version++;
    }

    private void Apply(CustomerRegistered customerRegistered)
    {
        Id = customerRegistered.CustomerId;
        FirstName = customerRegistered.FirstName;
        LastName = customerRegistered.LastName;
        EmailAddress = customerRegistered.EmailAddress;
        InvoiceAddress = customerRegistered.InvoiceAddress;
        ShippingAddress = customerRegistered.ShippingAddress;

        Version++;
    }
}