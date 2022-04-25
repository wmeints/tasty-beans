using RecommendCoffee.Shared.Application;
using RecommendCoffee.Shared.Domain;

namespace RecommendCoffee.Shared.Application;

public interface IEventPublisher
{
    Task PublishEventsAsync(IEnumerable<IDomainEvent> events);
}