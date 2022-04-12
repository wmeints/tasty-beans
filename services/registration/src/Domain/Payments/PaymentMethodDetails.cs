namespace RecommendCoffee.Registration.Domain.Payments;

public record PaymentMethodDetails(CardType CardType, string CardNumber, string ExpirationDate,
    string SecurityCode, string CardHolderName);