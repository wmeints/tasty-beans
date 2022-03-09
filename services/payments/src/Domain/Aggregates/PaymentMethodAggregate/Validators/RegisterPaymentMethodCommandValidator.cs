using System.Text.RegularExpressions;
using FluentValidation;
using RecommendCoffee.Payments.Domain.Aggregates.PaymentMethodAggregate.Commands;

namespace RecommendCoffee.Payments.Domain.Aggregates.PaymentMethodAggregate.Validators;

public class RegisterPaymentMethodCommandValidator: AbstractValidator<RegisterPaymentMethodCommand>
{
    private static Regex ExpirationDatePattern = new Regex(@"^(0[0-9]|1[0-2])\/[0-9]{2}$");
    
    public RegisterPaymentMethodCommandValidator()
    {
        RuleFor(x => x.CardHolderName)
            .NotEmpty()
            .MaximumLength(150);
        
        RuleFor(x => x.SecurityCode)
            .MinimumLength(3)
            .MaximumLength(3);
        
        RuleFor(x => x.ExpirationDate)
            .MinimumLength(5)
            .MaximumLength(5)
            .Matches(ExpirationDatePattern);

        RuleFor(x => x.CustomerId).NotEqual(Guid.Empty);
        RuleFor(x => x.Id).NotEqual(Guid.Empty);
    }
}