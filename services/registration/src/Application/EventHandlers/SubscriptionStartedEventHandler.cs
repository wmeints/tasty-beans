using RecommendCoffee.Registration.Application.IntegrationEvents;
using RecommendCoffee.Registration.Domain.Common;
using RecommendCoffee.Registration.Domain.Customers;
using RecommendCoffee.Registration.Domain.Registrations;
using RecommendCoffee.Registration.Domain.Subscriptions;

namespace RecommendCoffee.Registration.Application.EventHandlers;

public class SubscriptionStartedEventHandler
{
    private readonly IStateStore _stateStore;
    private readonly ICustomerManagement _customerManagement;
    private readonly ISubscriptions _subscriptions;

    public SubscriptionStartedEventHandler(IStateStore stateStore, ICustomerManagement customerManagement, ISubscriptions subscriptions)
    {
        _stateStore = stateStore;
        _customerManagement = customerManagement;
        _subscriptions = subscriptions;
    }

    public async Task HandleAsync(SubscriptionStartedEvent evt)
    {
        var stateData = await _stateStore.Get<RegistrationData>(evt.CustomerId.ToString());
        
        var registration = new Domain.Registrations.Registration(
            stateData, _customerManagement, _subscriptions, _stateStore);

        await registration.CompleteSubscriptionRegistration();
    }
}