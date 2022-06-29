using TastyBeans.Catalog.Domain.Aggregates.ProductAggregate;
using TastyBeans.Catalog.Domain.Aggregates.ProductAggregate.Commands;
using TastyBeans.Shared.Application;
using TastyBeans.Shared.Domain;

namespace TastyBeans.Catalog.Application.CommandHandlers;

public class CompleteTasteTestCommandHandler
{
    private readonly IEventStore _eventStore;
    private readonly IEventPublisher _eventPublisher;

    public CompleteTasteTestCommandHandler(IEventPublisher eventPublisher, IEventStore eventStore)
    {
        _eventPublisher = eventPublisher;
        _eventStore = eventStore;
    }

    public async Task ExecuteAsync(CompleteTasteTest cmd)
    {
        var product = await _eventStore.GetAsync<Product>(cmd.ProductId);

        if (product == null)
        {
            throw new AggregateNotFoundException("Can't find the specified product");
        }

        product.CompleteTasteTest(cmd);
        
        if (product.IsValid)
        {
            await _eventStore.AppendAsync(product.Id, product.Version, product.PendingDomainEvents);
            await _eventPublisher.PublishEventsAsync(product.PendingDomainEvents);
        }
    }
}