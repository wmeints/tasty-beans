namespace TastyBeans.Shared.Domain;

public class EventSourcedAggregateRoot<T> : AggregateRoot<T>
{
    private static readonly EventSourcedAggregateModel<T> _metaModel = new();

    public EventSourcedAggregateRoot(T id) : base(id)
    {
    }

    public EventSourcedAggregateRoot(T id, IEnumerable<IDomainEvent> events):base(id)
    {
        
    }

    protected override void Emit(IDomainEvent evt)
    {
        base.Emit(evt);
        ApplyEvent(evt);
    }

    private void ApplyEvent(IDomainEvent evt)
    {
        if (!_metaModel.EventHandlers.TryGetValue(evt.GetType(), out var eventHandlerMethod))
        {
            throw new Exception($"No event handler method defined in the aggregate for event type {evt.GetType().FullName}");
        }

        eventHandlerMethod.Invoke(this, new object? [] { evt });
    }
}