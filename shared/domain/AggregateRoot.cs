namespace TastyBeans.Shared.Domain;

public abstract class AggregateRoot<T>: Entity<T>
{
    private readonly List<IDomainEvent> _events = new();
    
    protected AggregateRoot(T id) : base(id)
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
        _events.Add(evt);
    }
}