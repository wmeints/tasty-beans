using MediatR;
using TastyBeans.Catalog.Application.Commands;
using TastyBeans.Catalog.Domain.Aggregates.ProductAggregate;
using TastyBeans.Shared.Application;
using TastyBeans.Shared.Domain;

namespace TastyBeans.Catalog.Application.CommandHandlers;

public class TasteTestProductCommandHandler : IRequestHandler<TasteTestProductCommand, TasteTestProductCommandResponse>
{
    private readonly IProductRepository _productRepository;
    private readonly IEventPublisher _eventPublisher;

    public TasteTestProductCommandHandler(IProductRepository productRepository, IEventPublisher eventPublisher)
    {
        _productRepository = productRepository;
        _eventPublisher = eventPublisher;
    }

    public async Task<TasteTestProductCommandResponse> Handle(TasteTestProductCommand request,
        CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.FindByIdAsync(request.ProductId);

        if (product == null)
        {
            return new TasteTestProductCommandResponse(false, Enumerable.Empty<BusinessRuleViolation>());
        }

        product.CompleteTasteTest(request.Taste, request.FlavorNotes, request.RoastLevel);

        if (product.IsValid)
        {
            await _productRepository.UpdateAsync(product);
            await _eventPublisher.PublishEventsAsync(product.PendingDomainEvents);
        }

        return new TasteTestProductCommandResponse(true, product.BusinessRuleViolations);
    }
}