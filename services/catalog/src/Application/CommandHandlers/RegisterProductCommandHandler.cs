using RecommendCoffee.Catalog.Application.IntegrationEvents;
using RecommendCoffee.Catalog.Application.Persistence;
using RecommendCoffee.Catalog.Domain.Aggregates.ProductAggregate;
using RecommendCoffee.Catalog.Domain.Aggregates.ProductAggregate.Commands;
using RecommendCoffee.Catalog.Domain.Projections.ProductInfoProjection;

namespace RecommendCoffee.Catalog.Application.CommandHandlers;

public class RegisterProductCommandHandler: ICommandHandler<RegisterProductCommand, RegisterProductCommandResponse>
{
    private readonly IEventStore<Product, Guid> _eventStore;
    private readonly IIntegrationEventPublisher _integrationEventPublisher;
    private readonly ProductInformationProjector _productInformationProjector;

    public RegisterProductCommandHandler(
        IEventStore<Product, Guid> eventStore, 
        ProductInformationProjector productInformationProjector, 
        IIntegrationEventPublisher integrationEventPublisher)
    {
        _eventStore = eventStore;
        _productInformationProjector = productInformationProjector;
        _integrationEventPublisher = integrationEventPublisher;
    }

    public async Task<RegisterProductCommandResponse> Execute(RegisterProductCommand cmd)
    {
        var product = Product.Register(cmd);

        await _eventStore.PersistAsync(product.Id, product.EventsToStore);
        await _integrationEventPublisher.PublishAsync(product.EventsToPublish);
        await _productInformationProjector.ApplyEvents(product.EventsToPublish);

        return new RegisterProductCommandResponse(product.Id);
    }
}
