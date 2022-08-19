using Jasper.Attributes;
using Jasper.Persistence.Sagas;
using TastyBeans.Registration.Api.Models;

namespace TastyBeans.Registration.Api.Application.IntegrationEvents;

[MessageIdentity("payments.payment-method.registered.v1")]
public record PaymentMethodRegisteredIntegrationEvent(
    Guid PaymentMethodId, 
    Guid CustomerId,
    string CardHolderName,
    string CardNumber,
    string SecurityCode,
    string ExpirationDate,
    CardType CardType)
{
    [SagaIdentity]
    public Guid CustomerId { get; init; } = CustomerId;
}
