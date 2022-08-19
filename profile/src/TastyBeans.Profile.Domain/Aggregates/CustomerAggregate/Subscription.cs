namespace TastyBeans.Profile.Domain.Aggregates.CustomerAggregate;

public record Subscription(DateTime StartDate, DateTime? EndDate = null);