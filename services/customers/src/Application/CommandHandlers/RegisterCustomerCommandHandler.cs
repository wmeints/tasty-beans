using RecommendCoffee.Customers.Domain.Aggregates.CustomerAggregate.Commands;

namespace RecommendCoffee.Customers.Application.CommandHandlers;

public class RegisterCustomerCommandHandler : ICommandHandler<RegisterCustomerCommand, RegisterCustomerCommandResponse>
{
    public async Task<RegisterCustomerCommandResponse> Execute(RegisterCustomerCommand request)
    {
        throw new NotImplementedException();
    }
}
