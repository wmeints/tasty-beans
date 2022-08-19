using Jasper;
using Jasper.Attributes;
using Marten;
using TastyBeans.Profile.Api.Application.IntegrationEvents;
using TastyBeans.Profile.Api.Application.ReadModels;
using TastyBeans.Profile.Domain.Aggregates.CustomerAggregate;

namespace TastyBeans.Profile.Api.Application.IntegrationEventHandlers;

public class MonthHasPassedIntegrationEventHandler
{
    [Transactional]
    public static async Task Handle(MonthHasPassed message, IDocumentSession session, IExecutionContext context)
    {
        var cancelledSubscriptions = await session
            .Query<CancelledSubscription>()
            .ToListAsync();

        foreach (var cancelledSubscription in cancelledSubscriptions)
        {
            var customer = await session.Events.AggregateStreamAsync<Customer>(cancelledSubscription.CustomerId);

            if (customer != null)
            {
                customer.EndSubscription();
                session.Events.Append(customer.Id, customer.Version, customer.PendingDomainEvents);

                await context.PublishAsync(new SubscriptionEndedIntegrationEvent(customer.Id));
            }
        }

        await session.SaveChangesAsync();
    }
}