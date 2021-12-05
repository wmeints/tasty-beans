using FluentValidation;
using RecommendCoffee.Customers.Domain.Aggregates.CustomerAggregate.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendCoffee.Customers.Domain.Aggregates.CustomerAggregate.Validators
{
    public class RegisterCustomerCommandValidator: AbstractValidator<RegisterCustomerCommand>
    {
        public RegisterCustomerCommandValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
            RuleFor(x=>x.LastName).NotEmpty().MaximumLength(100);

            RuleFor(x => x.InvoiceAddress).NotNull().ChildRules(x =>
            {
                x.RuleFor(y => y.StreetName).NotEmpty().MaximumLength(500);
                x.RuleFor(y => y.HouseNumber).NotEmpty().MaximumLength(20);
                x.RuleFor(y => y.ZipCode).NotEmpty().MaximumLength(20);
                x.RuleFor(y => y.City).NotEmpty().MaximumLength(100);
            });

            RuleFor(x=>x.ShippingAddress).NotNull().ChildRules(x =>
            {
                x.RuleFor(y => y.StreetName).NotEmpty().MaximumLength(500);
                x.RuleFor(y => y.HouseNumber).NotEmpty().MaximumLength(20);
                x.RuleFor(y => y.ZipCode).NotEmpty().MaximumLength(20);
                x.RuleFor(y => y.City).NotEmpty().MaximumLength(100);
            });
        }
    }
}
