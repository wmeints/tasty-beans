using RecommendCoffee.Ratings.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendCoffee.Ratings.Domain.Aggregates.RatingAggregate.Events
{
    [Topic("ratings.rating.registered.v1")]
    public record RatingRegisteredEvent(Guid RatingId, Guid UserId, Guid ProductId, int Value): IDomainEvent;
}
