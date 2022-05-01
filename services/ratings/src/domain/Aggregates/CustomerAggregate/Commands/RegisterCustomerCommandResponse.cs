using System.Diagnostics.CodeAnalysis;
using TastyBeans.Shared.Domain;

namespace TastyBeans.Ratings.Domain.Aggregates.CustomerAggregate.Commands;

public record RegisterCustomerCommandResponse(Customer? Customer, IEnumerable<ValidationError> Errors)
{
    [MemberNotNullWhen(true, nameof(Customer))]
    public bool IsValid => !Errors.Any();
}