using FluentValidation;
using TastyBeans.CustomerManagement.Domain.Aggregates.CustomerAggregate.Commands;

namespace TastyBeans.CustomerManagement.Domain.Aggregates.CustomerAggregate.Validators;

public class RegisterCustomerCommandValidator : AbstractValidator<RegisterCustomerCommand>
{
    public RegisterCustomerCommandValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.TelephoneNumber).NotEmpty().MaximumLength(13);
        RuleFor(x => x.EmailAddress).NotEmpty().MaximumLength(500);

        RuleFor(x=>x.InvoiceAddress).SetValidator(new AddressValidator());
        RuleFor(x=>x.ShippingAddress).SetValidator(new AddressValidator());
    }
}
