using RecommendCoffee.Subscriptions.Domain.Common;

namespace RecommendCoffee.Subscriptions.Application.Common;

public interface IEventPublisher
{
    Task PublishEventsAsync(IEnumerable<IDomainEvent> events);
}