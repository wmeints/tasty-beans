using TastyBeans.Shared.Domain;

namespace TastyBeans.Ratings.Domain.Aggregates.RatingAggregate.Events
{
    [Topic("ratings.rating.registered.v1")]
    public record RatingRegisteredEvent(Guid RatingId, Guid UserId, Guid ProductId, int Value): IDomainEvent;
}
