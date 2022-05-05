namespace TastyBeans.Simulation.Domain.Services.Ratings;

public interface IRatings
{
    Task RateProductAsync(Guid customerId, Guid productId, int value);
}