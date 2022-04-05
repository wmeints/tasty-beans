using RecommendCoffee.Ratings.Application.Common;
using RecommendCoffee.Ratings.Application.IntegrationEvents;
using RecommendCoffee.Ratings.Domain.Aggregates.ProductAggregate;
using RecommendCoffee.Ratings.Domain.Aggregates.ProductAggregate.Commands;

namespace RecommendCoffee.Ratings.Application.EventHandlers;

public class ProductRegisteredEventHandler
{
    private readonly IProductRepository _productRepository;

    public ProductRegisteredEventHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task HandleAsync(ProductRegisteredEvent evt)
    {
        var response = Product.Register(new RegisterProductCommand(evt.ProductId, evt.Name));

        if (!response.IsValid)
        {
            throw new EventValidationFailedException(response.Errors);
        }

        await _productRepository.InsertAsync(response.Product);
    }
}