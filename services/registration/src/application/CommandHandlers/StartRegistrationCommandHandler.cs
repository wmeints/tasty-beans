using TastyBeans.Registration.Domain.Customers;
using TastyBeans.Registration.Domain.Payments;
using TastyBeans.Registration.Domain.Registrations.Commands;
using TastyBeans.Registration.Domain.Subscriptions;
using TastyBeans.Shared.Domain;

namespace TastyBeans.Registration.Application.CommandHandlers;

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
        
        Metrics.RegistrationsStarted.Add(1);
    }
}