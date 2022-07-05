using NodaTime.TimeZones;

namespace TastyBeans.Shared.Domain;

public abstract class EventSourcedAggregateRoot
{
    private readonly List<object> _pendingDomainEvents = new();
    private readonly List<BusinessRuleViolation> _businessRuleViolations = new();

    public IReadOnlyCollection<object> PendingDomainEvents => _pendingDomainEvents.AsReadOnly();
    public IReadOnlyCollection<BusinessRuleViolation> BusinessRuleViolations => _businessRuleViolations.AsReadOnly();
    public long Version { get; protected set; } = 0L;
    public bool IsValid => !_businessRuleViolations.Any();

    public void ClearPendingDomainEvents()
    {
        _pendingDomainEvents.Clear();
    }
    
    protected EventSourcedAggregateRoot()
    {
    }

    protected void Emit(object evt)
    {
        if (TryApplyEvent(evt))
        {
            _pendingDomainEvents.Add(evt);
        }
    }

    protected void AddBusinessRuleViolation(string errorMessage)
    {
        _businessRuleViolations.Add(new(errorMessage));
    }

    protected abstract bool TryApplyEvent(object evt);
}