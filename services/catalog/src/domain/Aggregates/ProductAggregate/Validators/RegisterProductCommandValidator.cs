using FluentValidation;
using TastyBeans.Catalog.Domain.Aggregates.ProductAggregate.Commands;

namespace TastyBeans.Catalog.Domain.Aggregates.ProductAggregate.Validators;

public class RegisterProductCommandValidator: AbstractValidator<RegisterProductCommand>
{
    public RegisterProductCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(500);
        RuleFor(x => x.Description).NotEmpty();
        RuleFor(x => x.Variants).NotEmpty();
    }
}