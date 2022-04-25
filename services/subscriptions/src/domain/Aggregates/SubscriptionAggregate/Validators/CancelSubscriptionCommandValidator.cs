using FluentValidation;
using RecommendCoffee.Subscriptions.Domain.Aggregates.SubscriptionAggregate.Commands;

namespace RecommendCoffee.Subscriptions.Domain.Aggregates.SubscriptionAggregate.Validators;

public class CancelSubscriptionCommandValidator: AbstractValidator<CancelSubscriptionCommand>
{
    public CancelSubscriptionCommandValidator()
    {
        RuleFor(x => x.CustomerId).NotEqual(Guid.Empty);
    }
}