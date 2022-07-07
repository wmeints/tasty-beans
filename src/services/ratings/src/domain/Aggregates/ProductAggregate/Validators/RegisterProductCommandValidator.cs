using FluentValidation;
using TastyBeans.Ratings.Domain.Aggregates.ProductAggregate.Commands;

namespace TastyBeans.Ratings.Domain.Aggregates.ProductAggregate.Validators;

public class RegisterProductCommandValidator: AbstractValidator<RegisterProductCommand>
{
    public RegisterProductCommandValidator()
    {
        RuleFor(x => x.Id).NotEqual(Guid.Empty);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(500);
    }
}