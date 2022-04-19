using RecommendCoffee.Catalog.Application.Common;
using RecommendCoffee.Catalog.Domain.Aggregates.ProductAggregate;
using RecommendCoffee.Catalog.Domain.Aggregates.ProductAggregate.Commands;

namespace RecommendCoffee.Catalog.Application.CommandHandlers;

public class RegisterProductCommandHandler
{
    private readonly IEventPublisher _eventPublisher;
    private readonly IProductRepository _productRepository;

    public RegisterProductCommandHandler(IProductRepository productRepository, IEventPublisher eventPublisher)
    {
        _productRepository = productRepository;
        _eventPublisher = eventPublisher;
    }

    public async Task<RegisterProductCommandResponse> ExecuteAsync(RegisterProductCommand cmd)
    {
        using var activity = Activities.ExecuteCommand("RegisterProduct");
        var response = Product.Register(cmd);

        if (response.IsValid)
        {
            await _productRepository.InsertAsync(response.Product);
            await _eventPublisher.PublishEventsAsync(response.Events);
            
            Metrics.ProductsRegistered.Add(1);
        }

        return response;
    }
}