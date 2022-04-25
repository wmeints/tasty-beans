using RecommendCoffee.Registration.Domain.Payments;

namespace RecommendCoffee.Registration.Application.IntegrationEvents;

public record PaymentMethodRegisteredEvent(
    Guid Id,
    CardType CardType,
    string CardNumber,
    string ExpirationDate,
    string SecurityCode,
    string CardHolderName,
    Guid CustomerId);