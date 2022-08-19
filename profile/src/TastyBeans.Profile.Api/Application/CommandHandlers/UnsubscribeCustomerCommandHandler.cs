using Jasper;
using Jasper.Attributes;
using Marten;
using TastyBeans.Profile.Api.Application.Commands;
using TastyBeans.Profile.Api.Application.IntegrationEvents;
using TastyBeans.Profile.Api.Shared;
using TastyBeans.Profile.Domain.Aggregates.CustomerAggregate;

namespace TastyBeans.Profile.Api.Application.CommandHandlers;

public class UnsubscribeCustomerCommandHandler
{
    [Transactional]
    public static async Task Handle(UnsubscribeCustomerCommand message, IDocumentSession documentSession,
        IExecutionContext context)
    {
        var customer = await documentSession.Events.AggregateStreamAsync<Customer>(message.CustomerId);

        if (customer == null)
        {
            throw new AggregateNotFoundException($"Couldn't find customer with id {message.CustomerId}");
        }

        customer.Unsubscribe(message.SubscriptionEndDate);

        documentSession.Events.Append(customer.Id, customer.Version, customer.PendingDomainEvents);
        await documentSession.SaveChangesAsync();

        await context.PublishAsync(new SubscriptionCancelledIntegrationEvent(
            customer.Id, message.SubscriptionEndDate));
    }
}