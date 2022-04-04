using RecommendCoffee.Ratings.Domain.Common;

namespace RecommendCoffee.Ratings.Application.Common;

public interface IEventPublisher
{
    Task PublishEventsAsync(IEnumerable<IDomainEvent> events);
}