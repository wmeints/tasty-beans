using RecommendCoffee.Recommendations.Application.IntegrationEvents;
using RecommendCoffee.Recommendations.Domain.Aggregates.ProductAggregate;
using RecommendCoffee.Recommendations.Domain.Aggregates.ProductAggregate.Commands;

namespace RecommendCoffee.Recommendations.Application.EventHandlers;

public class ProductRegisteredEventHandler
{
    private readonly IProductRepository _productRepository;

    public ProductRegisteredEventHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task HandleAsync(ProductRegisteredEvent evt)
    {
        using var activity = Activities.HandleEvent("catalog.product.registered.v1");
        var response = Product.Register(new RegisterProductCommand(evt.ProductId, evt.Name));

        if (!response.IsValid)
        {
            throw new EventValidationFailedException(response.Errors);
        }

        await _productRepository.InsertAsync(response.Product);
    }
}