using RecommendCoffee.Catalog.Application.IntegrationEvents;
using RecommendCoffee.Catalog.Domain.Common;

namespace RecommendCoffee.Catalog.Infrastructure.IntegrationEvents;

public class IntegrationEventPublisher : IIntegrationEventPublisher
{
    public Task PublishAsync(IEnumerable<Event> @event)
    {
        return Task.CompletedTask;
    }
}
