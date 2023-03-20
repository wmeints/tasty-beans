using TastyBeans.Ratings.Application.IntegrationEvents;
using TastyBeans.Ratings.Domain.Aggregates.ProductAggregate;
using TastyBeans.Ratings.Domain.Aggregates.ProductAggregate.Commands;
using TastyBeans.Shared.Application;
using TastyBeans.Shared.Domain;

namespace TastyBeans.Ratings.Application.EventHandlers;

public class ProductUpdatedEventHandler
{
    private readonly IProductRepository _productRepository;

    public ProductUpdatedEventHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task HandleAsync(ProductUpdatedEvent evt)
    {
        using var activity = Activities.HandleEvent("catalog.product.updated.v1");
        var product = await _productRepository.FindByIdAsync(evt.ProductId);

        if (product == null)
        {
            throw new AggregateNotFoundException($"Could not find the specified product {evt.ProductId}");
        }

        var response = product.Update(new UpdateProductCommand(evt.Name));

        if (!response.IsValid)
        {
            throw new EventValidationFailedException(response.Errors);
        }

        await _productRepository.UpdateAsync(product);
    }
}