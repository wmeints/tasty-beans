using RecommendCoffee.CustomerManagement.Domain.Common;

namespace RecommendCoffee.CustomerManagement.Application.Common;

public interface IEventPublisher
{
    Task PublishEventsAsync(IEnumerable<IDomainEvent> events);
}