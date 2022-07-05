using MediatR;
using TastyBeans.Catalog.Application.Commands;
using TastyBeans.Catalog.Domain.Aggregates.ProductAggregate;
using TastyBeans.Shared.Application;
using TastyBeans.Shared.Domain;

namespace TastyBeans.Catalog.Application.CommandHandlers;

public class UpdateProductCommandHandler: IRequestHandler<UpdateProductCommand, UpdateProductCommandResponse>
{
    private readonly IProductRepository _productRepository;
    private readonly IEventPublisher _eventPublisher;

    public UpdateProductCommandHandler(IProductRepository productRepository, IEventPublisher eventPublisher)
    {
        _productRepository = productRepository;
        _eventPublisher = eventPublisher;
    }

    public async Task<UpdateProductCommandResponse> Handle(UpdateProductCommand request, CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.FindByIdAsync(request.ProductId);

        if (product == null)
        {
            return new UpdateProductCommandResponse(false, Enumerable.Empty<BusinessRuleViolation>());
        }

        product.UpdateProductDetails(request.Name, request.Description);

        if (product.IsValid)
        {
            await _productRepository.UpdateAsync(product);
            await _eventPublisher.PublishEventsAsync(product.PendingDomainEvents);
        }

        return new UpdateProductCommandResponse(true, product.BusinessRuleViolations);
    }
}