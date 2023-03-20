using FluentValidation;
using TastyBeans.Ratings.Domain.Aggregates.CustomerAggregate.Commands;

namespace TastyBeans.Ratings.Domain.Aggregates.CustomerAggregate.Validators;

public class RegisterCustomerCommandValidator: AbstractValidator<RegisterCustomerCommand>
{
    public RegisterCustomerCommandValidator()
    {
        RuleFor(x => x.Id).NotEqual(Guid.Empty);
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(100);
    }
}