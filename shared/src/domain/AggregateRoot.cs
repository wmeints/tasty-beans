namespace TastyBeans.Shared.Domain;

public abstract class AggregateRoot
{
    private readonly List<IDomainEvent> _events = new();
    
    protected AggregateRoot(Guid id)
    {
    }
    
    public long Version { get; private set; } = 0L;

    public IReadOnlyCollection<IDomainEvent> PendingDomainEvents => _events.AsReadOnly();

    public void ClearDomainEvents()
    {
        _events.Clear();
    }

    protected virtual void Emit(IDomainEvent evt)
    {
        if (TryApplyEvent(evt))
        {
            _events.Add(evt);
        }
    }

    protected abstract bool TryApplyEvent(IDomainEvent evt);
}