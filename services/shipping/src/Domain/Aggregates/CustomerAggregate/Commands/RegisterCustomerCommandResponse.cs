using System.Diagnostics.CodeAnalysis;
using RecommendCoffee.Shipping.Domain.Common;

namespace RecommendCoffee.Shipping.Domain.Aggregates.CustomerAggregate.Commands;

public record RegisterCustomerCommandResponse(Customer? Customer, IEnumerable<ValidationError> Errors)
{
    [MemberNotNullWhen(true, nameof(Customer))]
    public bool IsValid => !Errors.Any();
}