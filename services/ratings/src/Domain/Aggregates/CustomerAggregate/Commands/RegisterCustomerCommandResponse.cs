using System.Diagnostics.CodeAnalysis;
using RecommendCoffee.Ratings.Domain.Common;

namespace RecommendCoffee.Ratings.Domain.Aggregates.CustomerAggregate.Commands;

public record RegisterCustomerCommandResponse(Customer? Customer, IEnumerable<ValidationError> Errors)
{
    [MemberNotNullWhen(true, nameof(Customer))]
    public bool IsValid => !Errors.Any();
}