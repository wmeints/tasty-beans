using RecommendCoffee.Customers.Domain.Common;

namespace RecommendCoffee.Customers.Application.IntegrationEvents;

public interface IIntegrationEventPublisher
{
    Task PublishAsync(IEnumerable<Event> @event);
}
