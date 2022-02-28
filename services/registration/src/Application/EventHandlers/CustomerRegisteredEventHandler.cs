using RecommendCoffee.Registration.Application.Common;
using RecommendCoffee.Registration.Application.IntegrationEvents;
using RecommendCoffee.Registration.Domain.Customers;
using RecommendCoffee.Registration.Domain.Registrations;
using RecommendCoffee.Registration.Domain.Subscriptions;

namespace RecommendCoffee.Registration.Application.EventHandlers;

public class CustomerRegisteredEventHandler
{
    private readonly IStateStore _stateStore;
    private readonly ICustomerManagement _customerManagement;
    private readonly ISubscriptions _subscriptions;

    public CustomerRegisteredEventHandler(
        IStateStore stateStore, 
        ICustomerManagement customerManagement,
        ISubscriptions subscriptions)
    {
        _stateStore = stateStore;
        _customerManagement = customerManagement;
        _subscriptions = subscriptions;
    }

    public async Task HandleAsync(CustomerRegisteredEvent evt)
    {
        var stateData = await _stateStore.Get<RegistrationData>(evt.CustomerId.ToString());
        var registration = new Domain.Registrations.Registration(stateData, _customerManagement, _subscriptions);

        await registration.CompleteCustomerRegistrationAsync();
    }
}