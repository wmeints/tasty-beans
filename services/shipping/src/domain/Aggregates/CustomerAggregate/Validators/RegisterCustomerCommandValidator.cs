using FluentValidation;
using RecommendCoffee.Shipping.Domain.Aggregates.CustomerAggregate.Commands;

namespace RecommendCoffee.Ratings.Domain.Aggregates.CustomerAggregate.Validators;

public class RegisterCustomerCommandValidator: AbstractValidator<RegisterCustomerCommand>
{
    public RegisterCustomerCommandValidator()
    {
        RuleFor(x => x.Id).NotEqual(Guid.Empty);
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(100);
    }
}