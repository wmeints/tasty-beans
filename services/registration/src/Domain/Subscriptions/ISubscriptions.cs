namespace RecommendCoffee.Registration.Domain.Subscriptions;

public interface ISubscriptions
{
    Task RegisterSubscriptionAsync(RegisterSubscriptionRequest request);
}