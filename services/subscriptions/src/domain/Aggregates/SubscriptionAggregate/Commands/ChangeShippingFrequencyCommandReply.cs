namespace RecommendCoffee.Subscriptions.Domain.Aggregates.SubscriptionAggregate.Commands;

public record ChangeShippingFrequencyCommandReply(
    IEnumerable<ValidationError> Errors,
    IEnumerable<IDomainEvent> Events)
{
    public bool IsValid => !Errors.Any();
}