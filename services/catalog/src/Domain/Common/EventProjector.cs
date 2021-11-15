namespace RecommendCoffee.Catalog.Domain.Common;

public abstract class EventProjector
{
    public virtual async Task ApplyEvents(IEnumerable<Event> events)
    {
        foreach(var e in events)
        {
            await ApplyEvent(e);
        }
    }

    protected abstract Task ApplyEvent(Event @event);
}
