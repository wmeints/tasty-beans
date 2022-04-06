using RecommendCoffee.CustomerManagement.Domain.Common;
using System.Diagnostics.CodeAnalysis;

namespace RecommendCoffee.CustomerManagement.Domain.Aggregates.CustomerAggregate.Commands;

public record RegisterCustomerCommandReply(Customer? Customer, IEnumerable<ValidationError> Errors, IEnumerable<IDomainEvent> Events)
{
    [MemberNotNullWhen(true, nameof(Customer))]
    public bool IsValid => !Errors.Any();
}