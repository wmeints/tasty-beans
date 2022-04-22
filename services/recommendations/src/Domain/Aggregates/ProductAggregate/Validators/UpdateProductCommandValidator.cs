using FluentValidation;
using RecommendCoffee.Recommendations.Domain.Aggregates.ProductAggregate.Commands;

namespace RecommendCoffee.Recommendations.Domain.Aggregates.ProductAggregate.Validators;

public class UpdateProductCommandValidator: AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(500);
    }
}