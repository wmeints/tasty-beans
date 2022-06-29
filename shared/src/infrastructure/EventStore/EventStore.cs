using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using TastyBeans.Shared.Application;
using TastyBeans.Shared.Domain;

namespace TastyBeans.Shared.Infrastructure.EventStore;

public class EventStore : IEventStore
{
    private readonly EventStoreDbContext _eventStoreDbContext;

    public EventStore(EventStoreDbContext eventStoreDbContext)
    {
        _eventStoreDbContext = eventStoreDbContext;
    }

    public async Task<T?> GetAsync<T>(Guid aggregateId) where T : AggregateRoot
    {
        var eventRecords = await _eventStoreDbContext.DomainEvents
            .Where(x => x.AggregateId == aggregateId)
            .OrderBy(x => x.SequenceNumber)
            .ToListAsync();

        if (!eventRecords.Any())
        {
            return null;
        }

        var domainEvents = eventRecords
            .Select(eventRecord =>
            {
                var eventType = DomainEventRegistry.GetType(eventRecord.PayloadType);

                if (eventType == null)
                {
                    throw new InvalidOperationException(
                        $"Can't deserialize event payload type {eventRecord.PayloadType}. No mapping registered.");
                }

                var domainEvent = (IDomainEvent?)JsonSerializer.Deserialize(eventRecord.PayloadData, eventType);

                if (domainEvent == null)
                {
                    throw new InvalidOperationException($"Can't deserialize event payload for event {eventRecord.Id}");
                }

                return domainEvent;
            })
            .ToList();

        long expectedVersion = eventRecords.Max(x => x.SequenceNumber);

        return (T)Activator.CreateInstance(typeof(T), aggregateId, expectedVersion, domainEvents)!;
    }

    public async Task AppendAsync(Guid aggregateId, long expectedVersion, IEnumerable<IDomainEvent> domainEvents)
    {
        var currentVersion = expectedVersion;
        var records = new List<DomainEventRecord>();

        foreach (var domainEvent in domainEvents)
        {
            var payloadType = DomainEventRegistry.GetSchema(domainEvent.GetType());
            var payloadData = JsonSerializer.Serialize(domainEvent);

            if (payloadType == null)
            {
                throw new InvalidOperationException($"Can't serialize unknown event {domainEvent.GetType()}");
            }

            records.Add(new DomainEventRecord(0L, aggregateId,
                ++currentVersion, payloadType, payloadData));
        }

        await _eventStoreDbContext.AddRangeAsync(records);
    }

    public async Task SaveChangesAsync()
    {
        await _eventStoreDbContext.SaveChangesAsync();
    }
}