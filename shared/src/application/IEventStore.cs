using TastyBeans.Shared.Domain;

namespace TastyBeans.Shared.Application;

public interface IEventStore
{
    Task<T?> GetAsync<T>(Guid aggregateId) where T: AggregateRoot;
    Task AppendAsync(Guid aggregateId, long expectedVersion, IEnumerable<IDomainEvent> domainEvents);
    Task SaveChangesAsync();
}