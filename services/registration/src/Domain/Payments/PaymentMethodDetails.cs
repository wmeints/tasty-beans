namespace RecommendCoffee.Registration.Domain.Payments;

public record PaymentMethodDetails(Guid CustomerId, CardType CardType, string CardNumber, string ExpirationDate,
    string SecurityCode, string CardHolderName);