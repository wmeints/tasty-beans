using TastyBeans.Payments.Domain.Aggregates.PaymentMethodAggregate;

namespace TastyBeans.Payments.Api.Forms;

public record RegisterPaymentMethodForm(Guid CustomerId, string CardHolderName, string CardNumber,
    string SecurityCode, string ExpirationDate, CardType CardType);