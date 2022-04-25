using FluentValidation;
using RecommendCoffee.Shipping.Domain.Aggregates.ShippingOrderAggregate.Commands;

namespace RecommendCoffee.Shipping.Domain.Aggregates.ShippingOrderAggregate.Validators;

public class CreateShippingOrderCommandValidator: AbstractValidator<CreateShippingOrderCommand>
{
    public CreateShippingOrderCommandValidator()
    {
        
    }
}