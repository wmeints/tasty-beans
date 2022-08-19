namespace TastyBeans.Payments.Domain.Aggregates.PaymentMethodAggregate.Events;

public record PaymentMethodRegistered(Guid PaymentMethodId, Guid CustomerId, string CardHolderName, string CardNumber,
    string SecurityCode, string ExpirationDate, CardType CardType);