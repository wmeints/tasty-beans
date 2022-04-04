using Domain.Aggregates.ProductAggregate;
using Domain.Aggregates.ProductAggregate.Validators;
using RecommendCoffee.Catalog.Domain.Aggregates.ProductAggregate.Commands;
using RecommendCoffee.Catalog.Domain.Aggregates.ProductAggregate.Events;
using RecommendCoffee.Catalog.Domain.Aggregates.ProductAggregate.Validators;
using RecommendCoffee.Catalog.Domain.Common;

namespace RecommendCoffee.Catalog.Domain.Aggregates.ProductAggregate;

public class Product
{
    private Product()
    {
        Name = "";
        Description = "";
        Variants = Enumerable.Empty<ProductVariant>();
    }
    
    public Product(Guid id, string name, string description, IEnumerable<ProductVariant> variants)
    {
        Id = id;
        Name = name;
        Description = description;
        Variants = variants;
    }

    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public bool Discontinued { get; private set; }
    public List<string>? FlavorNotes { get; private set; }
    public string? Taste { get; private set; }
    public int? RoastLevel { get; private set; }
    public IEnumerable<ProductVariant> Variants { get; private set; }

    public UpdateProductCommandResponse Update(UpdateProductCommand cmd)
    {
        var validator = new UpdateProductCommandValidator();
        var validationResult = validator.Validate(cmd);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .Select(x => new ValidationError(x.PropertyName, x.ErrorMessage))
                .ToList();
            
            return new UpdateProductCommandResponse(errors, Enumerable.Empty<IDomainEvent>());
        }

        Name = cmd.Name;
        Description = cmd.Description;
        Variants = new List<ProductVariant>(cmd.Variants);

        var evt = new ProductUpdatedEvent(Id, Name, Description, Variants);

        return new UpdateProductCommandResponse(
            Enumerable.Empty<ValidationError>(),
            new IDomainEvent[] { evt });
    }

    public DiscontinueProductCommandResponse Discontinue(DiscontinueProductCommand cmd)
    {
        if (Discontinued)
        {
            return new DiscontinueProductCommandResponse(new[]
            {
                new ValidationError("", "Product is already discontinued.")
            }, Enumerable.Empty<IDomainEvent>());
        }

        Discontinued = true;

        var evt = new ProductDiscontinuedEvent(Id);

        return new DiscontinueProductCommandResponse(
            Enumerable.Empty<ValidationError>(), 
            new IDomainEvent[] { evt });
    }

    public TasteTestProductCommandResponse TasteTest(TasteTestProductCommand cmd)
    {
        var validator = new TasteTestCommandValidator();
        var validationResult = validator.Validate(cmd);

        if (!validationResult.IsValid)
        {
            return new TasteTestProductCommandResponse(
                validationResult.GetValidationErrors(),
                Enumerable.Empty<IDomainEvent>());
        }

        RoastLevel = cmd.RoastLevel;
        Taste = cmd.Taste;
        FlavorNotes = cmd.FlavorNotes.ToList();

        var tasteTestedEvt = new ProductTasteTestedEvent(Id, RoastLevel.Value, Taste, FlavorNotes);
        
        return new TasteTestProductCommandResponse(
            Enumerable.Empty<ValidationError>(),
            new IDomainEvent[] { tasteTestedEvt });
    }
    
    public static RegisterProductCommandResponse Register(RegisterProductCommand cmd)
    {
        var validator = new RegisterProductCommandValidator();
        var validationResult = validator.Validate(cmd);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .Select(x => new ValidationError(x.PropertyName, x.ErrorMessage))
                .ToList();
            
            return new RegisterProductCommandResponse(null, errors, Enumerable.Empty<IDomainEvent>());
        }

        var product = new Product(Guid.NewGuid(), cmd.Name, cmd.Description, cmd.Variants);
        var evt = new ProductRegisteredEvent(product.Id, product.Name, product.Description, product.Variants);

        return new RegisterProductCommandResponse(
            product, 
            Enumerable.Empty<ValidationError>(),
            new IDomainEvent[] { evt });
    }
}