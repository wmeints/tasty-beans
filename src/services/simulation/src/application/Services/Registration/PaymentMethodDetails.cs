namespace TastyBeans.Simulation.Application.Services.Registration;

public record PaymentMethodDetails(CardType CardType, string CardNumber, string ExpirationDate,
    string SecurityCode, string CardHolderName);