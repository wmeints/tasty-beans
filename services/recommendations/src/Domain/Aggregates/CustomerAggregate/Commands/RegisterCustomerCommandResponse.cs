using System.Diagnostics.CodeAnalysis;
using RecommendCoffee.Recommendations.Domain.Common;

namespace RecommendCoffee.Recommendations.Domain.Aggregates.CustomerAggregate.Commands;

public record RegisterCustomerCommandResponse(Customer? Customer, IEnumerable<ValidationError> Errors)
{
    [MemberNotNullWhen(true, nameof(Customer))]
    public bool IsValid => !Errors.Any();
}