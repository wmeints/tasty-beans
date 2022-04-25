namespace RecommendCoffee.Registration.Domain.Payments;

public interface IPayments
{
    Task RegisterPaymentMethodAsync(RegisterPaymentMethodRequest request);
}