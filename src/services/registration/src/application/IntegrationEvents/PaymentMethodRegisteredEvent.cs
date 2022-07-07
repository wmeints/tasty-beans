using TastyBeans.Registration.Domain.Payments;

namespace TastyBeans.Registration.Application.IntegrationEvents;

public record PaymentMethodRegisteredEvent(
    Guid Id,
    CardType CardType,
    string CardNumber,
    string ExpirationDate,
    string SecurityCode,
    string CardHolderName,
    Guid CustomerId);