using RecommendCoffee.Payments.Domain.Common;

namespace RecommendCoffee.Payments.Application.Common;

public interface IEventPublisher
{
    Task PublishEventsAsync(IEnumerable<IDomainEvent> events);
}