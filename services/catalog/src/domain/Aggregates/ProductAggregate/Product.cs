using FluentValidation;
using TastyBeans.Catalog.Domain.Aggregates.ProductAggregate.Commands;
using TastyBeans.Catalog.Domain.Aggregates.ProductAggregate.Events;
using TastyBeans.Shared.Domain;

namespace TastyBeans.Catalog.Domain.Aggregates.ProductAggregate;

public class Product : AggregateRoot
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public int? RoastLevel { get; private set; }
    public string? Taste { get; private set; }
    public IReadOnlyCollection<string>? FlavorNotes { get; private set; }
    public bool IsAvailable { get; private set; }
    public bool IsDiscontinued { get; private set; }

    public Product(Register cmd) : base(cmd.Id)
    {
        Emit(new Registered(Id, cmd.Name,cmd.Description));
    }

    public Product(Guid id, long version, IEnumerable<IDomainEvent> domainEvents) : base(id, version, domainEvents)
    {
    }

    public void CompleteTasteTest(CompleteTasteTest cmd)
    {
        if (IsAvailable)
        {
            throw new InvalidOperationException("Product is already available for shipping to subscribers");
        }

        if (IsDiscontinued)
        {
            throw new InvalidOperationException("Can't taste test a discontinued product");
        }
        
        Emit(new TasteTestCompleted(Id, cmd.RoastLevel,cmd.Taste,cmd.FlavorNotes));
    }

    public void Discontinue(Discontinue cmd)
    {
        if (IsDiscontinued)
        {
            throw new InvalidOperationException("Can't discontinue an already discontinued product");
        }
        
        Emit(new Discontinued(Id));
    }
    
    protected override bool TryApplyEvent(IDomainEvent evt)
    {
        switch (evt)
        {
            case Registered registered:
                Apply(registered);
                break;
            case Discontinued discontinued:
                Apply(discontinued);
                break;
            case TasteTestCompleted tasteTestCompleted:
                Apply(tasteTestCompleted);
                break;
            default:
                return false;
        }

        return true;
    }

    private void Apply(Registered registered)
    {
        Name = registered.Name;
        Description = registered.Description;
    }
    
    private void Apply(TasteTestCompleted tasteTestCompleted)
    {
        Taste = tasteTestCompleted.Taste;
        FlavorNotes = tasteTestCompleted.FlavorNotes.ToList().AsReadOnly();
        RoastLevel = tasteTestCompleted.RoastLevel;
        IsAvailable = true;
    }

    private void Apply(Discontinued discontinued)
    {
        IsAvailable = false;
        IsDiscontinued = true;
    }
}