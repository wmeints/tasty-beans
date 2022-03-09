namespace RecommendCoffee.Payments.Domain.Aggregates.PaymentMethodAggregate;

public interface IPaymentMethodRepository
{
    Task<PaymentMethod?> FindByCustomerIdAsync(Guid customerId);
    Task<int> InsertAsync(PaymentMethod paymentMethod);
    Task<int> UpdateAsync(PaymentMethod paymentMethod);
    Task<int> DeleteAsync(PaymentMethod paymentMethod);
}