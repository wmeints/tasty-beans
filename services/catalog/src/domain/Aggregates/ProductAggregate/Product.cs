using Marten.Events;
using TastyBeans.Catalog.Domain.Aggregates.ProductAggregate.Events;
using TastyBeans.Shared.Domain;

namespace TastyBeans.Catalog.Domain.Aggregates.ProductAggregate;

public class Product : EventSourcedAggregateRoot
{
    private Product()
    {
    }

    public Product(Guid id, string name, string description)
    {
        Id = id;
        Emit(new ProductRegisteredEvent(id, name, description));
    }

    public Guid Id { get; private set; }
    public string Name { get; private set; } = "";
    public string Description { get; private set; } = "";
    public bool Discontinued { get; private set; }
    public bool IsAvailable { get; private set; }
    public List<string>? FlavorNotes { get; private set; }
    public string? Taste { get; private set; }
    public int? RoastLevel { get; private set; }

    public void UpdateProductDetails(string name, string description)
    {
        Emit(new ProductUpdatedEvent(Id, name, description));
    }

    public void Discontinue()
    {
        Emit(new ProductDiscontinuedEvent(Id));
    }

    public void CompleteTasteTest(string taste, string[] flavorNotes, int roastLevel)
    {
        Emit(new ProductTasteTestedEvent(Id, taste, flavorNotes, roastLevel));
    }

    protected override bool TryApplyEvent(object evt)
    {
        switch (evt)
        {
            case ProductRegisteredEvent productRegisteredEvent:
                Apply(productRegisteredEvent);
                break;
            case ProductUpdatedEvent productUpdatedEvent:
                Apply(productUpdatedEvent);
                break;
            case ProductTasteTestedEvent productTasteTestedEvent:
                Apply(productTasteTestedEvent);
                break;
            case ProductDiscontinuedEvent productDiscontinuedEvent:
                Apply(productDiscontinuedEvent);
                break;
            default:
                return false;
        }

        return true;
    }

    private void Apply(ProductRegisteredEvent productRegisteredEvent, IEvent? @event = null)
    {
        Version = @event?.Version ?? Version;

        Name = productRegisteredEvent.Name;
        Description = productRegisteredEvent.Description;
    }

    private void Apply(ProductUpdatedEvent productUpdatedEvent, IEvent? @event = null)
    {
        Version = @event?.Version ?? Version;

        Name = productUpdatedEvent.Name;
        Description = productUpdatedEvent.Description;
    }

    private void Apply(ProductTasteTestedEvent productTasteTestedEvent, IEvent? @event = null)
    {
        Version = @event?.Version ?? Version;

        Taste = productTasteTestedEvent.Taste;
        FlavorNotes = productTasteTestedEvent.FlavorNotes.ToList();
        RoastLevel = productTasteTestedEvent.RoastLevel;
        IsAvailable = true;
    }
    
    private void Apply(ProductDiscontinuedEvent productDiscontinuedEvent, IEvent? @event = null)
    {
        Version = @event?.Version ?? Version;

        Discontinued = true;
        IsAvailable = false;
    }
}