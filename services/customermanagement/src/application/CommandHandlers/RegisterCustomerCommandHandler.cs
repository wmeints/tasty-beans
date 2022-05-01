using TastyBeans.CustomerManagement.Domain.Aggregates.CustomerAggregate;
using TastyBeans.CustomerManagement.Domain.Aggregates.CustomerAggregate.Commands;
using TastyBeans.Shared.Application;

namespace TastyBeans.CustomerManagement.Application.CommandHandlers;

public class RegisterCustomerCommandHandler
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IEventPublisher _eventPublisher;

    public RegisterCustomerCommandHandler(ICustomerRepository customerRepository, IEventPublisher eventPublisher)
    {
        _customerRepository = customerRepository;
        _eventPublisher = eventPublisher;
    }

    public async Task<RegisterCustomerCommandReply> ExecuteAsync(RegisterCustomerCommand command)
    {
        using var activity = Activities.ExecuteCommand("RegisterCustomer");
        
        var response = Customer.Register(command);

        if(response.IsValid)
        {
            await _customerRepository.InsertAsync(response.Customer);
            await _eventPublisher.PublishEventsAsync(response.Events);
            
            Metrics.CustomerRegistered.Add(1);
        }

        return response;
    }
}
