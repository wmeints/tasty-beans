using TastyBeans.Shared.Domain;

namespace TastyBeans.Shared.Application;

public interface IEventStore
{
    Task<T> GetOrCreateAsync<T>(Guid aggregateId);
    Task AppendAsync(Guid aggregateId, long expectedVersion, IEnumerable<IDomainEvent> domainEvents);
    Task SaveChangesAsync();
}