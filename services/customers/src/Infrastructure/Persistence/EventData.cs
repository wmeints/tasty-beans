using System.Diagnostics.CodeAnalysis;

namespace RecommendCoffee.Customers.Infrastructure.Persistence;

public class EventData<TKey>
{
    private EventData()
    {

    }

    public EventData(Guid eventId, [DisallowNull] TKey aggregateId, string eventType, string data, DateTime dateCreated)
    {
        EventId = eventId;
        EventType = eventType;
        AggregateId = aggregateId;
        Data = data;
        Timestamp = dateCreated;
    }

    public Guid EventId { get; private set; }

    [NotNull]
    public TKey? AggregateId { get; private set; }

    [NotNull]
    public string? EventType { get; private set; }

    [NotNull]
    public string? Data { get; private set; }

    public DateTime Timestamp { get; private set; }
}
