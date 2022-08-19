using Marten;
using Marten.Events.Projections;
using TastyBeans.Profile.Api.Application.ReadModels;
using TastyBeans.Profile.Domain.Aggregates.CustomerAggregate.Events;

namespace TastyBeans.Profile.Api.Application.Projections;

public class CancelledSubscriptionProjection : EventProjection
{
    public CancelledSubscriptionProjection()
    {
        Project<CustomerSubscriptionEnded>(OnSubscriptionEnded);
        Project<CustomerUnsubscribed>(OnCustomerUnsubscribed);
    }

    private void OnCustomerUnsubscribed(CustomerUnsubscribed evt, IDocumentOperations ops)
    {
        ops.Insert(new CancelledSubscription(evt.CustomerId, evt.SubscriptionEndDate));
    }

    private void OnSubscriptionEnded(CustomerSubscriptionEnded evt, IDocumentOperations ops)
    {
        ops.DeleteWhere<CancelledSubscription>(x => x.CustomerId == evt.CustomerId);
    }
}