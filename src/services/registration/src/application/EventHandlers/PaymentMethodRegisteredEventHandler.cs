using TastyBeans.Registration.Application.IntegrationEvents;
using TastyBeans.Registration.Domain.Customers;
using TastyBeans.Registration.Domain.Payments;
using TastyBeans.Registration.Domain.Registrations;
using TastyBeans.Registration.Domain.Subscriptions;
using TastyBeans.Shared.Domain;

namespace TastyBeans.Registration.Application.EventHandlers;

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