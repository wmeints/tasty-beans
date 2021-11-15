using FluentValidation;
using RecommendCoffee.Catalog.Domain.Aggregates.ProductAggregate.Commands;

namespace RecommendCoffee.Catalog.Domain.Aggregates.ProductAggregate.Validators;

internal class UpdateProductDetailsCommandValidator : AbstractValidator<UpdateProductDetailsCommand>
{
    public UpdateProductDetailsCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(500);
        RuleFor(x => x.Description).NotEmpty();
        RuleFor(x => x.Variants).NotEmpty();

        RuleForEach(x => x.Variants)
            .Must(x => x.Weight == 250 || x.Weight == 500 || x.Weight == 1000)
            .WithMessage("Products are sold in packages of 250, 500, or 1000 grams.");
    }
}
