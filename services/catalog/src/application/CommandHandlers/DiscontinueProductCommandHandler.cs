using TastyBeans.Catalog.Domain.Aggregates.ProductAggregate;
using TastyBeans.Catalog.Domain.Aggregates.ProductAggregate.Commands;
using TastyBeans.Shared.Application;
using TastyBeans.Shared.Domain;

namespace TastyBeans.Catalog.Application.CommandHandlers;

public class DiscontinueProductCommandHandler
{
    private readonly IEventStore _eventStore;
    private readonly IEventPublisher _eventPublisher;

    public DiscontinueProductCommandHandler(IEventStore eventStore, IEventPublisher eventPublisher)
    {
        _eventStore = eventStore;
        _eventPublisher = eventPublisher;
    }

    public async Task ExecuteAsync(Discontinue cmd)
    {
        using var activity = Activities.ExecuteCommand("DiscontinueProduct");
        var product = await _eventStore.GetAsync<Product>(cmd.ProductId);

        if (product == null)
        {
            throw new AggregateNotFoundException("Can't find the specified product");
        }

        product.Discontinue(cmd);

        if (product.IsValid)
        {
            await _eventStore.AppendAsync(product.Id, product.Version, product.PendingDomainEvents);
            await _eventPublisher.PublishEventsAsync(product.PendingDomainEvents);
        }
    }
}
