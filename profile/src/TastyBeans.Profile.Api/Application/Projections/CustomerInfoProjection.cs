using System.Diagnostics.Tracing;
using Marten.Events.Aggregation;
using TastyBeans.Profile.Api.Application.ReadModels;
using TastyBeans.Profile.Domain.Aggregates.CustomerAggregate.Events;

namespace TastyBeans.Profile.Api.Application.Projections;

public class CustomerInfoProjection: SingleStreamAggregation<CustomerInfo>
{
    public static CustomerInfo Create(CustomerRegistered evt) => new CustomerInfo(
            evt.CustomerId, evt.FirstName, evt.LastName, evt.ShippingAddress, evt.InvoiceAddress,
            evt.EmailAddress, SubscriptionStatus.Inactive, null, null);

    public static CustomerInfo Apply(CustomerSubscribed evt, CustomerInfo current) => current with
    {
        SubscriptionStartDate = evt.SubscriptionStartDate,
        SubscriptionStatus = SubscriptionStatus.Active
    };

    public static CustomerInfo Apply(CustomerUnsubscribed evt, CustomerInfo current) => current with
    {
        SubscriptionStatus = SubscriptionStatus.CancellationPending,
        SubscriptionEndDate = evt.SubscriptionEndDate
    };
    
    public static CustomerInfo Apply(CustomerSubscriptionEnded evt, CustomerInfo current) => current with
    {
        SubscriptionStatus = SubscriptionStatus.Inactive,
        SubscriptionEndDate = null,
        SubscriptionStartDate = null
    };
}