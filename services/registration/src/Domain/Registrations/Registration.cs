using RecommendCoffee.Registration.Domain.Common;
using RecommendCoffee.Registration.Domain.Customers;
using RecommendCoffee.Registration.Domain.Registrations.Commands;
using RecommendCoffee.Registration.Domain.Subscriptions;
using Stateless;

namespace RecommendCoffee.Registration.Domain.Registrations;

public class Registration
{
    private readonly RegistrationData _data;
    private readonly ICustomerManagement _customerManagement;
    private readonly ISubscriptions _subscriptions;
    private StateMachine<RegistrationState, RegistrationTrigger> _stateMachine;

    public Registration(ICustomerManagement customerManagement, ISubscriptions subscriptions, IStateStore stateStore)
    {
        _data = new RegistrationData { State = RegistrationState.NotStarted };
        _customerManagement = customerManagement;
        _subscriptions = subscriptions;
        _stateMachine = CreateStateMachine(_data, stateStore);
    }
    
    public Registration(
        RegistrationData data, 
        ICustomerManagement customerManagement,
        ISubscriptions subscriptions,
        IStateStore stateStore)
    {
        _data = data;
        _customerManagement = customerManagement;
        _subscriptions = subscriptions;

        _stateMachine = CreateStateMachine(data, stateStore);
    }

    public RegistrationData Data => _data;
    
    public async Task StartAsync(StartRegistrationCommand command)
    {
        _data.CustomerId = command.CustomerId;
        _data.CustomerDetails = command.CustomerDetails;
        _data.SubscriptionDetails = command.SubscriptionDetails;
        _data.PaymentMethodDetails = command.PaymentMethodDetails;
        
        await _stateMachine.FireAsync(RegistrationTrigger.RegisterCustomerDetails);
    }

    public async Task CompleteCustomerRegistrationAsync()
    {
        await _stateMachine.FireAsync(RegistrationTrigger.CompleteCustomerRegistration);
    }

    public async Task CompletePaymentMethodRegistration()
    {
        await _stateMachine.FireAsync(RegistrationTrigger.CompletePaymentMethodRegistration);
    }

    public async Task CompleteSubscriptionRegistration()
    {
        await _stateMachine.FireAsync(RegistrationTrigger.CompleteSubscriptionRegistration);
    }

    private StateMachine<RegistrationState, RegistrationTrigger> CreateStateMachine(
        RegistrationData data, IStateStore stateStore)
    {
        var stateMachine = new StateMachine<RegistrationState, RegistrationTrigger>(
            () => data.State, 
            state =>
            {
                _data.State = state;
                stateStore.Put(_data.CustomerId.ToString(), _data);
            });

        stateMachine
            .Configure(RegistrationState.NotStarted)
            .Permit(RegistrationTrigger.RegisterCustomerDetails, RegistrationState.WaitingForCustomerRegistration);

        stateMachine.Configure(RegistrationState.WaitingForCustomerRegistration)
            .OnEntryFromAsync(RegistrationTrigger.RegisterCustomerDetails, RegisterCustomerDetails)
            .Permit(RegistrationTrigger.CompleteCustomerRegistration, 
                RegistrationState.WaitingForPaymentMethodRegistration);

        stateMachine.Configure(RegistrationState.WaitingForPaymentMethodRegistration)
            .OnEntryFromAsync(RegistrationTrigger.CompleteCustomerRegistration, RegisterPaymentMethod)
            .Permit(RegistrationTrigger.CompletePaymentMethodRegistration, 
                RegistrationState.WaitingForSubscriptionRegistration);

        stateMachine.Configure(RegistrationState.WaitingForSubscriptionRegistration)
            .OnEntryAsync(RegisterSubscriptionDetails)
            .Permit(RegistrationTrigger.CompleteSubscriptionRegistration,
                RegistrationState.Completed);

        return stateMachine;
    }
    
    private async Task RegisterSubscriptionDetails()
    {
        var request = new RegisterSubscriptionRequest(
            _data.CustomerId,
            _data.SubscriptionDetails.ShippingFrequency,
            _data.SubscriptionDetails.Kind);

        await _subscriptions.RegisterSubscriptionAsync(request);
    }

    private async Task RegisterCustomerDetails()
    {
        var request = new RegisterCustomerRequest(
            _data.CustomerId,
            _data.CustomerDetails.FirstName,
            _data.CustomerDetails.LastName,
            _data.CustomerDetails.EmailAddress,
            _data.CustomerDetails.TelephoneNumber,
            _data.CustomerDetails.InvoiceAddress,
            _data.CustomerDetails.ShippingAddress);

        await _customerManagement.RegisterCustomerAsync(request);
    }

    private async Task RegisterPaymentMethod()
    {
        //TODO: Call the payment service
    }
    
    
}