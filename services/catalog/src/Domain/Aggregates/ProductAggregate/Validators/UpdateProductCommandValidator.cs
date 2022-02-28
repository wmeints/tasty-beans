using FluentValidation;
using RecommendCoffee.Catalog.Domain.Aggregates.ProductAggregate.Commands;

namespace Domain.Aggregates.ProductAggregate.Validators;

public class UpdateProductCommandValidator: AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(500);
        RuleFor(x => x.Description).NotEmpty();
        RuleFor(x => x.Variants).NotEmpty();
    }
}