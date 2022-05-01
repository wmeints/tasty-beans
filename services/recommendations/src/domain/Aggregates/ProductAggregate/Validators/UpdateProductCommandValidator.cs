using FluentValidation;
using TastyBeans.Recommendations.Domain.Aggregates.ProductAggregate.Commands;

namespace TastyBeans.Recommendations.Domain.Aggregates.ProductAggregate.Validators;

public class UpdateProductCommandValidator: AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(500);
    }
}