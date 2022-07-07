using Marten;
using MediatR;
using TastyBeans.Catalog.Application.Commands;
using TastyBeans.Catalog.Domain.Aggregates.ProductAggregate;
using TastyBeans.Shared.Application;
using TastyBeans.Shared.Domain;

namespace TastyBeans.Catalog.Application.CommandHandlers;

public class TasteTestProductCommandHandler : IRequestHandler<TasteTestProductCommand, TasteTestProductCommandResponse>
{
    private readonly IDocumentSession _documentSession;
    private readonly IEventPublisher _eventPublisher;

    public TasteTestProductCommandHandler(IDocumentSession documentSession, IEventPublisher eventPublisher)
    {
        _documentSession = documentSession;
        _eventPublisher = eventPublisher;
    }

    public async Task<TasteTestProductCommandResponse> Handle(TasteTestProductCommand request,
        CancellationToken cancellationToken = default)
    {
        var product = await _documentSession.Events.AggregateStreamAsync<Product>(
            request.ProductId, token: cancellationToken);

        if (product == null)
        {
            return new TasteTestProductCommandResponse(false, Enumerable.Empty<BusinessRuleViolation>());
        }

        product.CompleteTasteTest(request.Taste, request.FlavorNotes, request.RoastLevel);

        if (product.IsValid)
        {
            await _documentSession.Events.AppendOptimistic(
                request.ProductId, cancellationToken, product.PendingDomainEvents.ToArray());

            await _documentSession.SaveChangesAsync(cancellationToken);

            await _eventPublisher.PublishEventsAsync(product.PendingDomainEvents);
        }

        return new TasteTestProductCommandResponse(true, product.BusinessRuleViolations);
    }
}