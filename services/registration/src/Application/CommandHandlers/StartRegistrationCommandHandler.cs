using RecommendCoffee.Registration.Application.Common;
using RecommendCoffee.Registration.Domain.Customers;
using RecommendCoffee.Registration.Domain.Registrations.Commands;
using RecommendCoffee.Registration.Domain.Subscriptions;

namespace RecommendCoffee.Registration.Application.CommandHandlers;

public class StartRegistrationCommandHandler
{
    private readonly IStateStore _stateStore;
    private readonly ICustomerManagement _customerManagement;
    private readonly ISubscriptions _subscriptions;

    public StartRegistrationCommandHandler(
        IStateStore stateStore,
        ICustomerManagement customerManagement,
        ISubscriptions subscriptions)
    {
        _stateStore = stateStore;
        _customerManagement = customerManagement;
        _subscriptions = subscriptions;
    }

    public async Task ExecuteAsync(StartRegistrationCommand command)
    {
        var registration = new Domain.Registrations.Registration(_customerManagement, _subscriptions);
        await registration.StartAsync(command);
        
        await _stateStore.Put(registration.Data.CustomerId.ToString(), registration.Data);
    }
}