namespace TastyBeans.Registration.Api.Models;

public record PaymentMethodInfo(
    string CardHolderName,
    string CardNumber,
    string SecurityCode,
    string ExpirationDate,
    CardType CardType);