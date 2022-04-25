using FluentValidation;
using RecommendCoffee.Ratings.Domain.Aggregates.ProductAggregate.Commands;

namespace RecommendCoffee.Ratings.Domain.Aggregates.ProductAggregate.Validators;

public class UpdateProductCommandValidator: AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(500);
    }
}