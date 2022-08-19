using Jasper.Attributes;
using TastyBeans.Payments.Domain.Aggregates.PaymentMethodAggregate;

namespace TastyBeans.Payments.Api.Application.IntegrationEvents;

[MessageIdentity("payments.payment-method.registered.v1")]
public record PaymentMethodRegisteredIntegrationEvent(
    Guid PaymentMethodId, 
    Guid CustomerId,
    string CardHolderName,
    string CardNumber,
    string SecurityCode,
    string ExpirationDate,
    CardType CardType);
