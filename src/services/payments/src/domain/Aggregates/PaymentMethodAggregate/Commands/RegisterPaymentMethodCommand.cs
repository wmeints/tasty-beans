namespace RecommendCoffee.Payments.Domain.Aggregates.PaymentMethodAggregate.Commands;

public record RegisterPaymentMethodCommand(
    Guid Id,
    CardType CardType,
    string CardNumber,
    string ExpirationDate,
    string SecurityCode,
    string CardHolderName,
    Guid CustomerId);