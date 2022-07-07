using Marten;
using MediatR;
using TastyBeans.Catalog.Application.Commands;
using TastyBeans.Catalog.Domain.Aggregates.ProductAggregate;
using TastyBeans.Shared.Application;
using TastyBeans.Shared.Domain;

namespace TastyBeans.Catalog.Application.CommandHandlers;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, UpdateProductCommandResponse>
{
    private readonly IDocumentSession _documentSession;
    private readonly IEventPublisher _eventPublisher;

    public UpdateProductCommandHandler(IDocumentSession documentSession, IEventPublisher eventPublisher)
    {
        _documentSession = documentSession;
        _eventPublisher = eventPublisher;
    }

    public async Task<UpdateProductCommandResponse> Handle(UpdateProductCommand request,
        CancellationToken cancellationToken = default)
    {
        var product = await _documentSession.Events.AggregateStreamAsync<Product>(
            request.ProductId, token: cancellationToken);

        if (product == null)
        {
            return new UpdateProductCommandResponse(false, Enumerable.Empty<BusinessRuleViolation>());
        }

        product.UpdateProductDetails(request.Name, request.Description);

        if (product.IsValid)
        {
            await _documentSession.Events.AppendOptimistic(
                product.Id, cancellationToken, product.PendingDomainEvents.ToArray());

            await _documentSession.SaveChangesAsync(cancellationToken);
            await _eventPublisher.PublishEventsAsync(product.PendingDomainEvents);
        }

        return new UpdateProductCommandResponse(true, product.BusinessRuleViolations);
    }
}