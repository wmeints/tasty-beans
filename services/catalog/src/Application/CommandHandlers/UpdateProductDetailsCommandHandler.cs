using RecommendCoffee.Catalog.Application.IntegrationEvents;
using RecommendCoffee.Catalog.Application.Persistence;
using RecommendCoffee.Catalog.Domain.Aggregates.ProductAggregate;
using RecommendCoffee.Catalog.Domain.Aggregates.ProductAggregate.Commands;

namespace RecommendCoffee.Catalog.Application.CommandHandlers;

public class UpdateProductDetailsCommandHandler : ICommandHandler<UpdateProductDetailsCommand>
{
    private readonly IEventStore<Product, Guid> _eventStore;
    private readonly IIntegrationEventPublisher _integrationEventPublisher;

    public UpdateProductDetailsCommandHandler(
        IEventStore<Product, Guid> eventStore,
        IIntegrationEventPublisher integrationEventPublisher)
    {
        _eventStore = eventStore;
        _integrationEventPublisher = integrationEventPublisher;
    }

    public async Task Execute(UpdateProductDetailsCommand request)
    {
        var productEvents = await _eventStore.LoadAsync(request.Id);

        if (!productEvents.Any())
        {
            throw AggregateNotFoundException.Create<Product, Guid>(request.Id);
        }

        var product = Product.Load(request.Id, productEvents);
        product.UpdateDetails(request);

        await _eventStore.PersistAsync(product.Id, product.EventsToStore);
        await _integrationEventPublisher.PublishAsync(product.EventsToPublish);
    }
}
