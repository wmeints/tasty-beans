namespace RecommendCoffee.Registration.Domain.Customers;

public interface ICustomerManagement
{
    Task RegisterCustomerAsync(RegisterCustomerRequest request);
}