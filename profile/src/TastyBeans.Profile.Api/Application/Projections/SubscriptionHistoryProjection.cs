using Marten.Events.Projections;
using TastyBeans.Profile.Api.Application.ReadModels;
using TastyBeans.Profile.Domain.Aggregates.CustomerAggregate.Events;

namespace TastyBeans.Profile.Api.Application.Projections;

public class SubscriptionHistoryProjection: EventProjection
{
    public SubscriptionHistoryProjection()
    {
        Project<CustomerSubscriptionEnded>((evt, ops) =>
            ops.Insert(new SubscriptionHistoryItem(
                Guid.NewGuid(), 
                evt.CustomerId, 
                evt.StartDate,
                evt.EndDate)));
    }
}