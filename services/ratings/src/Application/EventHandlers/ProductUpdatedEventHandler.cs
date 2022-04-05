using RecommendCoffee.Ratings.Application.Common;
using RecommendCoffee.Ratings.Application.IntegrationEvents;
using RecommendCoffee.Ratings.Domain.Aggregates.ProductAggregate;
using RecommendCoffee.Ratings.Domain.Aggregates.ProductAggregate.Commands;
using RecommendCoffee.Ratings.Domain.Common;

namespace RecommendCoffee.Ratings.Application.EventHandlers;

public class ProductUpdatedEventHandler
{
    private readonly IProductRepository _productRepository;

    public ProductUpdatedEventHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task HandleAsync(ProductUpdatedEvent evt)
    {
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