using FluentValidation;
using TastyBeans.Shipping.Domain.Aggregates.ProductAggregate.Commands;

namespace TastyBeans.Shipping.Domain.Aggregates.ProductAggregate.Validators;

public class RegisterProductCommandValidator: AbstractValidator<RegisterProductCommand>
{
    public RegisterProductCommandValidator()
    {
        RuleFor(x => x.Id).NotEqual(Guid.Empty);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(500);
    }
}