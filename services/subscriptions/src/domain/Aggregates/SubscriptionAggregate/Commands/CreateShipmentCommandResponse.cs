using TastyBeans.Shared.Domain;

namespace TastyBeans.Subscriptions.Domain.Aggregates.SubscriptionAggregate.Commands;

public record CreateShipmentCommandResponse(IEnumerable<ValidationError> Errors, IEnumerable<IDomainEvent> Events)
{
    public bool IsValid => !Errors.Any();
}
