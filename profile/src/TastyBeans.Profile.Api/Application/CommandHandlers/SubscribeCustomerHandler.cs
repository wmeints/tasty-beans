using Jasper;
using Jasper.Attributes;
using Jasper.Persistence.Sagas;
using Marten;
using TastyBeans.Profile.Api.Application.Commands;
using TastyBeans.Profile.Api.Application.IntegrationEvents;
using TastyBeans.Profile.Api.Shared;
using TastyBeans.Profile.Domain.Aggregates.CustomerAggregate;

namespace TastyBeans.Profile.Api.Application.CommandHandlers;

public class SubscribeCustomerCommandHandler
{
    [Transactional]
    public static async ValueTask Handle(SubscribeCustomerCommand message, IDocumentSession documentSession,
        IExecutionContext context)
    {
        var customer = await documentSession.Events.AggregateStreamAsync<Customer>(message.CustomerId);

        if (customer == null)
        {
            throw new AggregateNotFoundException($"Couldn't find customer with id {message.CustomerId}");
        }

        customer.Subscribe(message.SubscriptionStartDate);

        documentSession.Events.Append(customer.Id, customer.Version, customer.PendingDomainEvents);
        await documentSession.SaveChangesAsync();

        await context.PublishAsync(new SubscriptionStartedIntegrationEvent(
            customer.Id, message.SubscriptionStartDate));
    }
}