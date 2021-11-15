using RecommendCoffee.Catalog.Domain.Common;

namespace RecommendCoffee.Catalog.Application.IntegrationEvents;

public interface IIntegrationEventPublisher
{
    Task PublishAsync(IEnumerable<Event> @event);
}
