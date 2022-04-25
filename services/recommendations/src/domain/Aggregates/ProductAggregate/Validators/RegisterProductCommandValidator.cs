using FluentValidation;
using RecommendCoffee.Recommendations.Domain.Aggregates.ProductAggregate.Commands;

namespace RecommendCoffee.Recommendations.Domain.Aggregates.ProductAggregate.Validators;

public class RegisterProductCommandValidator: AbstractValidator<RegisterProductCommand>
{
    public RegisterProductCommandValidator()
    {
        RuleFor(x => x.Id).NotEqual(Guid.Empty);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(500);
    }
}