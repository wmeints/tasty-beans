using Marten;
using MediatR;
using TastyBeans.Catalog.Application.Commands;
using TastyBeans.Catalog.Domain.Aggregates.ProductAggregate;
using TastyBeans.Shared.Application;
using TastyBeans.Shared.Domain;

namespace TastyBeans.Catalog.Application.CommandHandlers;

public class
    DiscontinueProductCommandHandler : IRequestHandler<DiscontinueProductCommand, DiscontinueProductCommandResponse>
{
    private readonly IDocumentSession _documentSession;
    private readonly IEventPublisher _eventPublisher;

    public DiscontinueProductCommandHandler(IDocumentSession documentSession, IEventPublisher eventPublisher)
    {
        _documentSession = documentSession;
        _eventPublisher = eventPublisher;
    }

    public async Task<DiscontinueProductCommandResponse> Handle(DiscontinueProductCommand request,
        CancellationToken cancellationToken = default)
    {
        var product = await _documentSession.Events.AggregateStreamAsync<Product>(
            request.ProductId, token: cancellationToken);

        if (product == null)
        {
            return new DiscontinueProductCommandResponse(false, Enumerable.Empty<BusinessRuleViolation>());
        }

        product.Discontinue();

        if (product.IsValid)
        {
            await _documentSession.Events.AppendOptimistic(
                product.Id, cancellationToken, product.PendingDomainEvents.ToArray());

            await _documentSession.SaveChangesAsync(cancellationToken);
            await _eventPublisher.PublishEventsAsync(product.PendingDomainEvents);
        }

        return new DiscontinueProductCommandResponse(true, product.BusinessRuleViolations);
    }
}