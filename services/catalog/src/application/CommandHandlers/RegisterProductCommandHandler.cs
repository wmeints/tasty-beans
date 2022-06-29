using TastyBeans.Catalog.Domain.Aggregates.ProductAggregate;
using TastyBeans.Catalog.Domain.Aggregates.ProductAggregate.Commands;
using TastyBeans.Shared.Application;

namespace TastyBeans.Catalog.Application.CommandHandlers;

public class RegisterProductCommandHandler
{
    private readonly IEventStore _eventStore;
    private readonly IEventPublisher _eventPublisher;

    public RegisterProductCommandHandler(IEventStore eventStore, IEventPublisher eventPublisher)
    {
        _eventStore = eventStore;
        _eventPublisher = eventPublisher;
    }

    public async Task ExecuteAsync(Register cmd)
    {
        var product = new Product(cmd);

        if (product.IsValid)
        {
            await _eventStore.AppendAsync(product.Id, product.Version, product.PendingDomainEvents);
            await _eventPublisher.PublishEventsAsync(product.PendingDomainEvents);
        }
    }
}