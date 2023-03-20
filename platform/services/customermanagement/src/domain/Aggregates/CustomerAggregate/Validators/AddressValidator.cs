using FluentValidation;

namespace TastyBeans.CustomerManagement.Domain.Aggregates.CustomerAggregate.Validators;

public class AddressValidator : AbstractValidator<Address>
{
    public AddressValidator()
    {
        RuleFor(x => x.Street).NotEmpty().MaximumLength(100);
        RuleFor(x => x.HouseNumber).NotEmpty().MaximumLength(20);
        RuleFor(x => x.PostalCode).NotEmpty().MaximumLength(20);
        RuleFor(x => x.City).NotEmpty().MaximumLength(100);
        RuleFor(x => x.CountryCode).NotEmpty().MaximumLength(10);
    }
}
