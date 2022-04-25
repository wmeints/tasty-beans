using RecommendCoffee.Catalog.Domain.Aggregates.ProductAggregate;
using RecommendCoffee.Catalog.Domain.Aggregates.ProductAggregate.Commands;

namespace RecommendCoffee.Catalog.Application.CommandHandlers;

public class UpdateProductCommandHandler
{
    private readonly IProductRepository _productRepository;
    private readonly IEventPublisher _eventPublisher;

    public UpdateProductCommandHandler(IProductRepository productRepository, IEventPublisher eventPublisher)
    {
        _productRepository = productRepository;
        _eventPublisher = eventPublisher;
    }

    public async Task<UpdateProductCommandResponse> ExecuteAsync(UpdateProductCommand cmd)
    {
        using var activity = Activities.ExecuteCommand("UpdateProduct");
        var product = await _productRepository.FindByIdAsync(cmd.ProductId);

        if (product == null)
        {
            throw new AggregateNotFoundException("Can't find the specified product");
        }

        var response = product.Update(cmd);

        if (response.IsValid)
        {
            await _productRepository.UpdateAsync(product);
            await _eventPublisher.PublishEventsAsync(response.Events);
        }

        return response;
    }
}