namespace TastyBeans.Registration.Domain.Payments;

public record RegisterPaymentMethodRequest(
    Guid CustomerId, CardType CardType, string CardNumber, 
    string ExpirationDate, string SecurityCode, string CardHolderName);