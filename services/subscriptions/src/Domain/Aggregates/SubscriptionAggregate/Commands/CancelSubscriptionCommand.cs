namespace RecommendCoffee.Subscriptions.Domain.Aggregates.SubscriptionAggregate.Commands;

public record CancelSubscriptionCommand(Guid CustomerId);