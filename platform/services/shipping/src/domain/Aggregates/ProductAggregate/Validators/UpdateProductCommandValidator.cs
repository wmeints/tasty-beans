using FluentValidation;
using TastyBeans.Shipping.Domain.Aggregates.ProductAggregate.Commands;

namespace TastyBeans.Shipping.Domain.Aggregates.ProductAggregate.Validators;

public class UpdateProductCommandValidator: AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(500);
    }
}