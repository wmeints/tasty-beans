using TastyBeans.Shared.Domain;

namespace TastyBeans.Shared.Application;

public interface IEventPublisher
{
    Task PublishEventsAsync(IEnumerable<IDomainEvent> events);
}