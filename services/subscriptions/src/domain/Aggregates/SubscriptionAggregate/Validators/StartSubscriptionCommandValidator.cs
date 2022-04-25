using FluentValidation;
using RecommendCoffee.Subscriptions.Domain.Aggregates.SubscriptionAggregate.Commands;

namespace RecommendCoffee.Subscriptions.Domain.Aggregates.SubscriptionAggregate.Validators;

public class StartSubscriptionCommandValidator: AbstractValidator<StartSubscriptionCommand>
{
    public StartSubscriptionCommandValidator()
    {
        RuleFor(x => x.CustomerId).NotEqual(Guid.Empty);
    }
}