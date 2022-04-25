using RecommendCoffee.Registration.Application.IntegrationEvents;
using RecommendCoffee.Registration.Domain.Customers;
using RecommendCoffee.Registration.Domain.Payments;
using RecommendCoffee.Registration.Domain.Registrations;
using RecommendCoffee.Registration.Domain.Subscriptions;

namespace RecommendCoffee.Registration.Application.EventHandlers;

public class PaymentMethodRegisteredEventHandler
{
    private readonly IStateStore _stateStore;
    private readonly ICustomerManagement _customerManagement;
    private readonly ISubscriptions _subscriptions;
    private readonly IPayments _payments;

    public PaymentMethodRegisteredEventHandler(IStateStore stateStore, ICustomerManagement customerManagement, ISubscriptions subscriptions, IPayments payments)
    {
        _stateStore = stateStore;
        _customerManagement = customerManagement;
        _subscriptions = subscriptions;
        _payments = payments;
    }

    public async Task HandleAsync(PaymentMethodRegisteredEvent evt)
    {
        var stateData = await _stateStore.Get<RegistrationData>(evt.CustomerId.ToString());
        
        var registration = new Domain.Registrations.Registration(
                stateData, 
                _customerManagement, 
                _subscriptions, 
                _stateStore,
                _payments);

        await registration.CompletePaymentMethodRegistrationAsync();
    }
}