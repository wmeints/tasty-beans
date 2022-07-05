using TastyBeans.Catalog.Application.Commands;
using TastyBeans.Catalog.Domain.Aggregates.ProductAggregate;
using TastyBeans.Shared.Application;

namespace TastyBeans.Catalog.Application.CommandHandlers;

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
        var product = new Product(Guid.NewGuid(), cmd.Name, cmd.Description);

        if (product.IsValid)
        {
            await _productRepository.InsertAsync(product);
            await _eventPublisher.PublishEventsAsync(product.PendingDomainEvents);
        }

        return new RegisterProductCommandResponse(product, product.BusinessRuleViolations);
    }
}