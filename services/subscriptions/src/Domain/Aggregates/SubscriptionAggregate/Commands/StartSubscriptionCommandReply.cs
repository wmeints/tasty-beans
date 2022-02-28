using System.Diagnostics.CodeAnalysis;
using RecommendCoffee.Subscriptions.Domain.Common;

namespace RecommendCoffee.Subscriptions.Domain.Aggregates.SubscriptionAggregate.Commands;

public record StartSubscriptionCommandReply(
    Subscription? Subscription, 
    IEnumerable<ValidationError> Errors, 
    IEnumerable<IDomainEvent> Events)
{
    [MemberNotNullWhen(true, nameof(Subscription))]
    public bool IsValid => !Errors.Any();
}