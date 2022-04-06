using RecommendCoffee.CustomerManagement.Application.Common;
using RecommendCoffee.CustomerManagement.Domain.Aggregates.CustomerAggregate;
using RecommendCoffee.CustomerManagement.Domain.Aggregates.CustomerAggregate.Commands;

namespace RecommendCoffee.CustomerManagement.Application.CommandHandlers;

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
        var response = Customer.Register(command);

        if(response.IsValid)
        {
            await _customerRepository.InsertAsync(response.Customer);
            await _eventPublisher.PublishEventsAsync(response.Events);
        }

        return response;
    }
}
