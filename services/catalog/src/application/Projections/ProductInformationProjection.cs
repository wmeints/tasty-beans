using Marten;
using Marten.Events.Projections;
using TastyBeans.Catalog.Domain.Aggregates.ProductAggregate.Events;

namespace TastyBeans.Catalog.Application.Projections;

public class ProductInformationProjection: EventProjection
{
    public ProductInformationProjection()
    {
        Project<ProductRegisteredEvent>(OnProductRegistered);
        Project<ProductUpdatedEvent>(OnProductUpdated);
        Project<ProductDiscontinuedEvent>(OnProductDiscontinued);
        Project<ProductTasteTestedEvent>(OnProductTasteTested);
    }

    private void OnProductTasteTested(ProductTasteTestedEvent evt, IDocumentOperations operations)
    {
        var product = operations.Load<ProductInfo>(evt.ProductId);

        if (product == null)
        {
            return;
        }

        product.Taste = evt.Taste;
        product.FlavorNotes = evt.FlavorNotes;
        product.RoastLevel = evt.RoastLevel;
        
        operations.Update(product);
    }

    private void OnProductDiscontinued(ProductDiscontinuedEvent evt, IDocumentOperations operations)
    {
        var product = operations.Load<ProductInfo>(evt.ProductId);

        if (product != null)
        {
            operations.Delete(product);
        }
    }

    private void OnProductUpdated(ProductUpdatedEvent evt, IDocumentOperations operations)
    {
        var product =  operations.Load<ProductInfo>(evt.ProductId);

        if (product == null)
        {
            return;
        }

        product.Name = evt.Name;
        product.Description = evt.Description;
        
        operations.Update(product);
    }

    private void OnProductRegistered(ProductRegisteredEvent evt, IDocumentOperations operations)
    {
        operations.Insert(new ProductInfo
        {
            Id = evt.ProductId,
            Name = evt.Name,
            Description = evt.Description
        });
    }
}