using RecommendCoffee.Ratings.Application.IntegrationEvents;
using RecommendCoffee.Ratings.Domain.Aggregates.ProductAggregate;
using RecommendCoffee.Ratings.Domain.Aggregates.ProductAggregate.Commands;

namespace RecommendCoffee.Ratings.Application.EventHandlers;

public class ProductDiscontinuedEventHandler
{
    private readonly IProductRepository _productRepository;

    public ProductDiscontinuedEventHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task HandleAsync(ProductDiscontinuedEvent evt)
    {
        using var activity = Activities.HandleEvent("catalog.product.discontinued.v1");
        var product = await _productRepository.FindByIdAsync(evt.ProductId);

        if (product == null)
        {
            throw new AggregateNotFoundException($"The product {evt.ProductId} could not be found.");
        }

        var response = product.Discontinue(new DiscontinueProductCommand());
        
        await _productRepository.InsertAsync(product);
    }
}