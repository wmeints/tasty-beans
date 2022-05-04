namespace TastyBeans.Simulation.Domain.Services.Subscriptions;

public interface ISubscriptions
{
    Task CancelSubscriptionAsync(Guid customerId);
}