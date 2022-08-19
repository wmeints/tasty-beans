using TastyBeans.Payments.Domain.Aggregates.PaymentMethodAggregate;

namespace TastyBeans.Payments.Api.Application.Commands;

public record RegisterPaymentMethodCommand(
    Guid PaymentMethodId,
    Guid CustomerId,
    string CardHolderName,
    string CardNumber,
    string SecurityCode,
    string ExpirationDate,
    CardType CardType);
