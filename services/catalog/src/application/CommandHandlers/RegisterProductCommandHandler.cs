using MediatR;
using TastyBeans.Catalog.Application.Commands;
using TastyBeans.Catalog.Domain.Aggregates.ProductAggregate;
using TastyBeans.Shared.Application;

namespace TastyBeans.Catalog.Application.CommandHandlers;

public class RegisterProductCommandHandler: IRequestHandler<RegisterProductCommand, RegisterProductCommandResponse>
{
    private readonly IEventPublisher _eventPublisher;
    private readonly IProductRepository _productRepository;

    public RegisterProductCommandHandler(IProductRepository productRepository, IEventPublisher eventPublisher)
    {
        _productRepository = productRepository;
        _eventPublisher = eventPublisher;
    }
    
    public async Task<RegisterProductCommandResponse> Handle(RegisterProductCommand request, CancellationToken cancellationToken = default)
    {
        var product = new Product(Guid.NewGuid(), request.Name, request.Description);

        if (product.IsValid)
        {
            await _productRepository.InsertAsync(product);
            await _eventPublisher.PublishEventsAsync(product.PendingDomainEvents);
        }

        return new RegisterProductCommandResponse(product, product.BusinessRuleViolations);
    }
}