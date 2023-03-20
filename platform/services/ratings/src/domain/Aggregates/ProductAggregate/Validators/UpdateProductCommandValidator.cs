using FluentValidation;
using TastyBeans.Ratings.Domain.Aggregates.ProductAggregate.Commands;

namespace TastyBeans.Ratings.Domain.Aggregates.ProductAggregate.Validators;

public class UpdateProductCommandValidator: AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(500);
    }
}