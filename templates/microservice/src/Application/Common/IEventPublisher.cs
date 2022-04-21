using RecommendCoffee.Templates.Microservice.Domain.Common;

namespace RecommendCoffee.Templates.Microservice.Application.Common;

public interface IEventPublisher
{
    Task PublishEventsAsync(IEnumerable<IDomainEvent> events);
}