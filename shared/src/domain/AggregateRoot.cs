namespace TastyBeans.Shared.Domain;

public abstract class AggregateRoot
{
    private readonly List<BusinessRuleViolation> _businessRuleViolations = new();
    private readonly List<IDomainEvent> _pendingDomainEvents = new();

    public Guid Id { get; private set; }

    public long Version { get; private set; }

    public IReadOnlyCollection<IDomainEvent> PendingDomainEvents => _pendingDomainEvents.AsReadOnly();
    public bool IsValid => !_businessRuleViolations.Any();

    protected AggregateRoot(Guid id)
    {
        Id = id;
        Version = 0L;
    }

    protected AggregateRoot(Guid id, long version, IEnumerable<IDomainEvent> domainEvents)
    {
        Version = version;
        Id = id;
    }

    public void ClearDomainEvents()
    {
        _pendingDomainEvents.Clear();
    }

    protected void Emit(IDomainEvent evt)
    {
        if (TryApplyEvent(evt))
        {
            _pendingDomainEvents.Add(evt);
        }
    }
    
    protected abstract bool TryApplyEvent(IDomainEvent evt);

    protected void AddBusinessRuleViolation(string errorMessage)
    {
        _businessRuleViolations.Add(new BusinessRuleViolation(errorMessage));
    }
}