using Marten;
using MediatR;
using TastyBeans.Catalog.Application.Commands;
using TastyBeans.Catalog.Domain.Aggregates.ProductAggregate;
using TastyBeans.Shared.Application;

namespace TastyBeans.Catalog.Application.CommandHandlers;

public class RegisterProductCommandHandler: IRequestHandler<RegisterProductCommand, RegisterProductCommandResponse>
{
    private readonly IDocumentSession _documentSession;
    private readonly IEventPublisher _eventPublisher;

    public RegisterProductCommandHandler(IDocumentSession documentSession, IEventPublisher eventPublisher)
    {
        _documentSession = documentSession;
        _eventPublisher = eventPublisher;
    }
    
    public async Task<RegisterProductCommandResponse> Handle(RegisterProductCommand request, CancellationToken cancellationToken = default)
    {
        var product = new Product(Guid.NewGuid(), request.Name, request.Description);

        if (product.IsValid)
        {
            _documentSession.Events.StartStream<Product>(product.Id, product.PendingDomainEvents);
            
            await _documentSession.SaveChangesAsync(cancellationToken);
            await _eventPublisher.PublishEventsAsync(product.PendingDomainEvents);
        }

        return new RegisterProductCommandResponse(product, product.BusinessRuleViolations);
    }
}