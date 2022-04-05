namespace RecommendCoffee.Ratings.Domain.Aggregates.CustomerAggregate;

public interface ICustomerRepository
{
    Task<int> InsertAsync(Customer customer);
    Task<bool> ExistsAsync(Guid id);
}