using Microsoft.EntityFrameworkCore;
using RecommendCoffee.Catalog.Application.Persistence;
using RecommendCoffee.Catalog.Domain.Common;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace RecommendCoffee.Catalog.Infrastructure.Persistence
{
    public class EventStore<TAggregateRoot, TKey> : IEventStore<TAggregateRoot, TKey> where TAggregateRoot : AggregateRoot<TKey>
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public EventStore(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<IEnumerable<Event>> LoadAsync(TKey id)
        {
            var eventStreamName = $"{typeof(TAggregateRoot).Name}Events";

            var eventRecords = await _applicationDbContext.Set<EventData<TKey>>(eventStreamName)
                .Where(x => x.AggregateId.Equals(id))
                .OrderBy(x => x.Timestamp)
                .ToListAsync();

            return eventRecords.Select(eventRecord =>
            {
                var eventType = Type.GetType(eventRecord.EventType) ??
                    throw new EventStoreException($"Unknown event type {eventRecord.EventType} encountered.");

                var eventInstance = JsonSerializer.Deserialize(eventRecord.Data, eventType);

                if (eventInstance == null)
                {
                    throw new EventStoreException($"Unable to deserialize event data for event {eventRecord.EventId}");
                }

                return (Event)eventInstance;
            }).ToList();
        }

        public async Task PersistAsync([DisallowNull] TKey aggregateId, IEnumerable<Event> events)
        {
            foreach(var evt in events)
            {
                var serializedEventData = JsonSerializer.Serialize(evt, evt.GetType());
                var eventTypeName = evt.GetType().AssemblyQualifiedName!;

                var set = _applicationDbContext.Set<EventData<TKey>>($"{typeof(TAggregateRoot).Name}Events");

                set.Add(new EventData<TKey>(evt.EventId, aggregateId, eventTypeName, serializedEventData, evt.OccurredOn));
            }

            await _applicationDbContext.SaveChangesAsync();
        }
    }
}
