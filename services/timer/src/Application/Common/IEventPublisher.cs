using RecommendCoffee.Timer.Domain.Common;

namespace RecommendCoffee.Timer.Application.Common;

public interface IEventPublisher
{
    Task PublishEventsAsync(IEnumerable<IDomainEvent> events);
}