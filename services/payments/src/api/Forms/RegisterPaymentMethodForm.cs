using RecommendCoffee.Payments.Domain.Aggregates.PaymentMethodAggregate;

namespace RecommendCoffee.Payments.Api.Forms;

public class RegisterPaymentMethodForm
{
    public CardType CardType { get; set; }
    public string CardNumber { get; set; }
    public string ExpirationDate { get; set; }
    public string SecurityCode { get; set; }
    public string CardHolderName { get; set; }
    public Guid CustomerId { get; set; }
}