using Jasper.Attributes;
using TastyBeans.Payments.Api.Application.Commands;
using TastyBeans.Payments.Api.Application.IntegrationEvents;
using TastyBeans.Payments.Domain.Aggregates.PaymentMethodAggregate;
using TastyBeans.Payments.Domain.Aggregates.PaymentMethodAggregate.Events;

namespace TastyBeans.Payments.Api.Application.CommandHandlers;

public class RegisterPaymentMethodCommandHandler
{
    [Transactional]
    public static async ValueTask Handle(
        RegisterPaymentMethodCommand message, IDocumentSession documentSession, IExecutionContext context)
    {
        var paymentMethod = PaymentMethod.Register(
            message.PaymentMethodId,
            message.CustomerId,
            message.CardHolderName,
            message.CardNumber,
            message.ExpirationDate,
            message.SecurityCode,
            message.CardType);

        documentSession.Events.Append(paymentMethod.Id, paymentMethod.Version, paymentMethod.PendingDomainEvents);
        await documentSession.SaveChangesAsync();

        await context.PublishAsync(new PaymentMethodRegisteredIntegrationEvent(
            paymentMethod.Id, paymentMethod.CustomerId, paymentMethod.CardHolderName, paymentMethod.CardNumber,
            paymentMethod.SecurityCode, paymentMethod.ExpirationDate, paymentMethod.CardType));
    }
}