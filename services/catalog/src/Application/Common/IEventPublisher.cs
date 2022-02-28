using RecommendCoffee.Catalog.Domain.Common;

namespace RecommendCoffee.Catalog.Application.Common;

public interface IEventPublisher
{
    Task PublishEventsAsync(IEnumerable<IDomainEvent> events);
}