using RecommendCoffee.Recommendations.Domain.Common;

namespace RecommendCoffee.Recommendations.Application.Common;

public interface IEventPublisher
{
    Task PublishEventsAsync(IEnumerable<IDomainEvent> events);
}