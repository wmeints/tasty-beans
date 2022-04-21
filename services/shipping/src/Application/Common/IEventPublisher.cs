using RecommendCoffee.Shipping.Domain.Common;

namespace RecommendCoffee.Shipping.Application.Common;

public interface IEventPublisher
{
    Task PublishEventsAsync(IEnumerable<IDomainEvent> events);
}