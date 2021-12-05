namespace RecommendCoffee.Customers.Domain.Common;

public abstract class AggregateRoot<TKey>
{
    private int _version;
    private bool _replaying;
    private List<Event> _eventsToPublish = new List<Event>();
    private List<Event> _eventsToStore = new List<Event>();

    protected AggregateRoot(TKey id)
    {
        Id = id;
    }

    public IEnumerable<Event> EventsToPublish => _eventsToPublish;

    public IEnumerable<Event> EventsToStore => _eventsToStore;

    public int Version => _version;

    public TKey Id { get; protected set; }

    public void Reset()
    {
        _eventsToPublish.Clear();
        _eventsToStore.Clear();
    }

    protected abstract Action GetEventHandler(Event @event);

    protected void BeginReplaying()
    {
        _replaying = true;
    }

    protected void EndReplaying()
    {
        _replaying = false;
    }

    protected void ReplayDomainEvents(IEnumerable<Event> events)
    {
        BeginReplaying();

        foreach (var e in events)
        {
            ApplyEvent(e);
        }

        EndReplaying();
    }

    protected void ApplyEvent(Event @event)
    {
        var handler = GetEventHandler(@event);

        if (handler != null)
        {
            handler();
        }

        _version++;

        if (!_replaying)
        {
            _eventsToStore.Add(@event);
        }
    }

    protected void PublishEvent(Event @event)
    {
        _eventsToPublish.Add(@event);
    }

    
}
