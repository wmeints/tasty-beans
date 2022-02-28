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

    public Registration(ICustomerManagement customerManagement, ISubscriptions subscriptions)
    {
        _data = new RegistrationData { State = RegistrationState.NotStarted };
        _customerManagement = customerManagement;
        _subscriptions = subscriptions;
        _stateMachine = CreateStateMachine(_data);
    }
    
    public Registration(
        RegistrationData data, 
        ICustomerManagement customerManagement,
        ISubscriptions subscriptions)
    {
        _data = data;
        _customerManagement = customerManagement;
        _subscriptions = subscriptions;

        _stateMachine = CreateStateMachine(data);
    }

    public RegistrationData Data => _data;
    
    public async Task StartAsync(StartRegistrationCommand command)
    {
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

    private StateMachine<RegistrationState, RegistrationTrigger> CreateStateMachine(RegistrationData data)
    {
        var stateMachine = new StateMachine<RegistrationState, RegistrationTrigger>(
            () => data.State, state => _data.State = state);

        stateMachine
            .Configure(RegistrationState.NotStarted)
            .OnEntryFromAsync(RegistrationTrigger.RegisterCustomerDetails, RegisterCustomerDetails);

        stateMachine.Configure(RegistrationState.WaitingForCustomerRegistration)
            .Permit(RegistrationTrigger.CompleteCustomerRegistration, RegistrationState.WaitingForPaymentMethodDetails);

        stateMachine.Configure(RegistrationState.WaitingForPaymentMethodDetails)
            .OnEntryAsync(RegisterPaymentMethod);

        stateMachine.Configure(RegistrationState.WaitingForPaymentMethodRegistration)
            .Permit(RegistrationTrigger.CompletePaymentMethodRegistration,
                RegistrationState.WaitingForSubscriptionDetails);

        stateMachine.Configure(RegistrationState.WaitingForSubscriptionDetails)
            .OnEntryAsync(RegisterSubscriptionDetails);

        stateMachine.Configure(RegistrationState.WaitingForSubscriptionRegistration)
            .Permit(RegistrationTrigger.CompleteSubscriptionRegistration, RegistrationState.Completed);

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
        //TODO: Implement the actual call to the payments service here.
        await CompletePaymentMethodRegistration();
    }
    
    
}