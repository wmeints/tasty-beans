using RecommendCoffee.Catalog.Domain.Aggregates.ProductAggregate.Commands;
using RecommendCoffee.Catalog.Domain.Aggregates.ProductAggregate.Events;
using RecommendCoffee.Catalog.Domain.Aggregates.ProductAggregate.Validators;
using System.Diagnostics.CodeAnalysis;

namespace RecommendCoffee.Catalog.Domain.Aggregates.ProductAggregate;

public class Product : AggregateRoot<Guid>
{
    private List<ProductVariant> _variants = new List<ProductVariant>();

    private Product(Guid id) : base(id)
    {

    }

    private Product(Guid id, IEnumerable<Event> domainEvents) : base(id)
    {
        ReplayDomainEvents(domainEvents);
    }

    [NotNull]
    public string? Name { get; private set; }

    [NotNull]
    public string? Description { get; private set; }

    public IReadOnlyCollection<ProductVariant> Variants => _variants.AsReadOnly();

    protected override Action GetEventHandler(Event @event) => @event switch
    {
        ProductRegistered evt => () => OnProductRegistered(evt),
        ProductDetailsUpdated evt => () => OnProductDetailsUpdated(evt),
        _ => throw new InvalidOperationException($"Requested event {@event.GetType()} is not supported by this aggregate.")
    };

    public static Product Load(Guid id, IEnumerable<Event> events)
    {
        return new Product(id, events);
    }

    public static Product Register(RegisterProductCommand cmd)
    {
        var validator = new RegisterProductCommandValidator();
        var validationResult = validator.Validate(cmd);

        if (!validationResult.IsValid)
        {
            var violations = validationResult.Errors.Select(x => new BusinessRuleViolation(x.PropertyName, x.ErrorMessage));
            throw new BusinessRulesViolationException(violations);
        }

        var productId = Guid.NewGuid();
        var product = new Product(productId);

        var productRegistered = new ProductRegistered(
            productId, cmd.Name, cmd.Description, cmd.Variants);

        product.ApplyEvent(productRegistered);
        product.PublishEvent(productRegistered);

        return product;
    }

    public void UpdateDetails(UpdateProductDetailsCommand cmd)
    {
        var validator = new UpdateProductDetailsCommandValidator();
        var validationResult = validator.Validate(cmd);

        if (!validationResult.IsValid)
        {
            var violations = validationResult.Errors.Select(x => new BusinessRuleViolation(x.PropertyName, x.ErrorMessage));
            throw new BusinessRulesViolationException(violations);
        }

        var evt = new ProductDetailsUpdated(Id, cmd.Name, cmd.Description, cmd.Variants);

        ApplyEvent(evt);
        PublishEvent(evt);
    }

    private void OnProductRegistered(ProductRegistered evt)
    {
        Name = evt.Name;
        Description = evt.Description;
        _variants = evt.Variants.ToList();
    }

    private void OnProductDetailsUpdated(ProductDetailsUpdated evt)
    {
        Name = evt.Name;
        Description = evt.Description;
        _variants = evt.Variants.ToList();
    }
}
