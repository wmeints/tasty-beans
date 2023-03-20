using FluentValidation;
using TastyBeans.Subscriptions.Domain.Aggregates.SubscriptionAggregate.Commands;

namespace TastyBeans.Subscriptions.Domain.Aggregates.SubscriptionAggregate.Validators;

public class StartSubscriptionCommandValidator: AbstractValidator<StartSubscriptionCommand>
{
    public StartSubscriptionCommandValidator()
    {
        RuleFor(x => x.CustomerId).NotEqual(Guid.Empty);
    }
}