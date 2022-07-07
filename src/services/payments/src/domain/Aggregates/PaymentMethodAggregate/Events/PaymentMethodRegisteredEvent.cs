using TastyBeans.Shared.Domain;

namespace RecommendCoffee.Payments.Domain.Aggregates.PaymentMethodAggregate.Events;

[Topic("payments.paymentmethod.registered.v1")]
public record PaymentMethodRegisteredEvent(
    Guid Id,
    CardType CardType,
    string CardNumber,
    string ExpirationDate,
    string SecurityCode,
    string CardHolderName,
    Guid CustomerId) : IDomainEvent;