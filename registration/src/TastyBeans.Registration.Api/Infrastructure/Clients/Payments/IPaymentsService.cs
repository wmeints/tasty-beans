namespace TastyBeans.Registration.Api.Infrastructure.Clients.Payments;

public interface IPaymentsService
{
    Task RegisterPaymentMethod(RegisterPaymentMethodRequest request);
}