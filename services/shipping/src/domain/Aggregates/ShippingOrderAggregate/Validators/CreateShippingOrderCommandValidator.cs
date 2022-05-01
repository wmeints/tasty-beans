using FluentValidation;
using TastyBeans.Shipping.Domain.Aggregates.ShippingOrderAggregate.Commands;

namespace TastyBeans.Shipping.Domain.Aggregates.ShippingOrderAggregate.Validators;

public class CreateShippingOrderCommandValidator: AbstractValidator<CreateShippingOrderCommand>
{
    public CreateShippingOrderCommandValidator()
    {
        
    }
}