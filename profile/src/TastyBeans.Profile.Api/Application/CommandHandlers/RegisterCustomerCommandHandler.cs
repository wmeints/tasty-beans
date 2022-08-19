using Jasper;
using Jasper.Attributes;
using Marten;
using TastyBeans.Profile.Api.Application.Commands;
using TastyBeans.Profile.Api.Application.IntegrationEvents;
using TastyBeans.Profile.Domain.Aggregates.CustomerAggregate;

namespace TastyBeans.Profile.Api.Application.CommandHandlers;

public class RegisterCustomerCommandHandler
{
    [Transactional]
    public static async ValueTask Handle(RegisterCustomerCommand message, IDocumentSession documentSession, IExecutionContext context)
    {
        var customer = Customer.Register(
            message.CustomerId, message.FirstName, message.LastName, message.ShippingAddress,
            message.InvoiceAddress, message.EmailAddress, message.SubscriptionStartDate);

        documentSession.Events.Append(customer.Id, customer.Version, customer.PendingDomainEvents);
        
        await documentSession.SaveChangesAsync();
        
        await context.PublishAsync(new CustomerRegisteredIntegrationEvent(
            message.CustomerId, message.FirstName, message.LastName, 
            message.ShippingAddress, message.InvoiceAddress,
            message.EmailAddress));

        await context.PublishAsync(new SubscriptionStartedIntegrationEvent(
            message.CustomerId, message.SubscriptionStartDate));
    }
}