using RecommendCoffee.Customers.Domain.Common;
using System.Diagnostics.CodeAnalysis;

namespace RecommendCoffee.Customers.Application.Persistence;

public interface IEventStore<TAggregateRoot, TKey> where TAggregateRoot : AggregateRoot<TKey>
{
    Task PersistAsync([DisallowNull] TKey aggregateId, IEnumerable<Event> events);
    Task<IEnumerable<Event>> LoadAsync([DisallowNull] TKey aggregateId);
}
