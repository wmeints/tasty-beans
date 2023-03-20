namespace TastyBeans.Registration.Domain.Payments;

public interface IPayments
{
    Task RegisterPaymentMethodAsync(RegisterPaymentMethodRequest request);
}