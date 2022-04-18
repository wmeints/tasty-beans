using RecommendCoffee.Catalog.Application.Common;
using RecommendCoffee.Catalog.Domain.Aggregates.ProductAggregate;
using RecommendCoffee.Catalog.Domain.Aggregates.ProductAggregate.Commands;
using RecommendCoffee.Catalog.Domain.Common;

namespace RecommendCoffee.Catalog.Application.CommandHandlers;

public class DiscontinueProductCommandHandler
{
    private readonly IProductRepository _productRepository;
    private readonly IEventPublisher _eventPublisher;

    public DiscontinueProductCommandHandler(IProductRepository productRepository, IEventPublisher eventPublisher)
    {
        _productRepository = productRepository;
        _eventPublisher = eventPublisher;
    }

    public async Task<DiscontinueProductCommandResponse> ExecuteAsync(DiscontinueProductCommand cmd)
    {
        using var activity = Activities.ExecuteCommand(nameof(DiscontinueProductCommand));
        var product = await _productRepository.FindByIdAsync(cmd.ProductId);

        if (product == null)
        {
            throw new AggregateNotFoundException("Can't find the specified product");
        }

        var response = product.Discontinue(cmd);

        if (response.IsValid)
        {
            await _productRepository.UpdateAsync(product);
            await _eventPublisher.PublishEventsAsync(response.Events);
            
            Metrics.ProductsDiscontinued.Add(1);
        }

        return response;
    }
}
