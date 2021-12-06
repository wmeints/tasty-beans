using System.Net.Http.Headers;
using RecommendCoffee.Customers.Application.IntegrationEvents;
using RecommendCoffee.Customers.Application.Persistence;
using RecommendCoffee.Customers.Domain.Aggregates.CustomerAggregate;
using RecommendCoffee.Customers.Domain.Aggregates.CustomerAggregate.Commands;
using RecommendCoffee.Customers.Domain.Projections.CustomerInfoProjection;

namespace RecommendCoffee.Customers.Application.CommandHandlers;

public class RegisterCustomerCommandHandler : ICommandHandler<RegisterCustomerCommand, RegisterCustomerCommandResponse>
{
    private readonly CustomerInformationProjector _customerInformationProjector;
    private readonly IEventStore<Customer, Guid> _eventStore;
    private readonly IIntegrationEventPublisher _integrationEventPublisher;

    public RegisterCustomerCommandHandler(CustomerInformationProjector customerInformationProjector,
        IEventStore<Customer, Guid> eventStore, IIntegrationEventPublisher integrationEventPublisher)
    {
        _customerInformationProjector = customerInformationProjector;
        _eventStore = eventStore;
        _integrationEventPublisher = integrationEventPublisher;
    }

    public async Task<RegisterCustomerCommandResponse> Execute(RegisterCustomerCommand request)
    {
        var customer = Customer.Register(request);

        await _eventStore.PersistAsync(customer.Id, customer.EventsToStore);
        await _integrationEventPublisher.PublishAsync(customer.EventsToPublish);
        await _customerInformationProjector.ApplyEvents(customer.EventsToPublish);

        return new RegisterCustomerCommandResponse(customer.Id);
    }
}