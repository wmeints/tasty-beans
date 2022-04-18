namespace RecommendCoffee.Portal.Client.Forms;

public record PaymentMethodDetails(CardType CardType, string CardNumber, string ExpirationDate,
    string SecurityCode, string CardHolderName);