using RecommendCoffee.Registration.Domain.Common;
using RecommendCoffee.Registration.Domain.Customers;
using RecommendCoffee.Registration.Domain.Payments;
using RecommendCoffee.Registration.Domain.Registrations.Commands;
using RecommendCoffee.Registration.Domain.Subscriptions;

namespace RecommendCoffee.Registration.Application.CommandHandlers;

public class StartRegistrationCommandHandler
{
    private readonly IStateStore _stateStore;
    private readonly ICustomerManagement _customerManagement;
    private readonly ISubscriptions _subscriptions;
    private readonly IPayments _payments;

    public StartRegistrationCommandHandler(
        IStateStore stateStore,
        ICustomerManagement customerManagement,
        ISubscriptions subscriptions, IPayments payments)
    {
        _stateStore = stateStore;
        _customerManagement = customerManagement;
        _subscriptions = subscriptions;
        _payments = payments;
    }

    public async Task ExecuteAsync(StartRegistrationCommand command)
    {
        var registration = new Domain.Registrations.Registration(
            _customerManagement, _subscriptions, _stateStore, _payments);
        
        await registration.StartAsync(command);
    }
}