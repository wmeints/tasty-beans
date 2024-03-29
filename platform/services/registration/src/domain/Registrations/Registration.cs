﻿using Stateless;
using TastyBeans.Registration.Domain.Customers;
using TastyBeans.Registration.Domain.Payments;
using TastyBeans.Registration.Domain.Registrations.Commands;
using TastyBeans.Registration.Domain.Subscriptions;
using TastyBeans.Shared.Domain;

namespace TastyBeans.Registration.Domain.Registrations;

public class Registration
{
    private readonly RegistrationData _data;
    private readonly ICustomerManagement _customerManagement;
    private readonly ISubscriptions _subscriptions;
    private readonly IPayments _payments;
    private StateMachine<RegistrationState, RegistrationTrigger> _stateMachine;

    public Registration(
        ICustomerManagement customerManagement, 
        ISubscriptions subscriptions,
        IStateStore stateStore,
        IPayments payments)
    {
        _data = new RegistrationData { State = RegistrationState.NotStarted };
        _customerManagement = customerManagement;
        _subscriptions = subscriptions;
        _payments = payments;
        _stateMachine = CreateStateMachine(_data, stateStore);
    }

    public Registration(
        RegistrationData data,
        ICustomerManagement customerManagement,
        ISubscriptions subscriptions,
        IStateStore stateStore, IPayments payments)
    {
        _data = data;
        _customerManagement = customerManagement;
        _subscriptions = subscriptions;
        _payments = payments;

        _stateMachine = CreateStateMachine(data, stateStore);
    }

    public RegistrationData Data => _data;

    public async Task StartAsync(StartRegistrationCommand command)
    {
        using var activity = Activities.StartRegistration(_data.State.ToString());
        
        _data.CustomerId = command.CustomerId;
        _data.CustomerDetails = command.CustomerDetails;
        _data.SubscriptionDetails = command.SubscriptionDetails;
        _data.PaymentMethodDetails = command.PaymentMethodDetails;

        await _stateMachine.FireAsync(RegistrationTrigger.RegisterCustomerDetails);
    }

    public async Task CompleteCustomerRegistrationAsync()
    {
        using var activity = Activities.CompleteCustomerRegistration(_data.State.ToString());
        await _stateMachine.FireAsync(RegistrationTrigger.CompleteCustomerRegistration);
    }

    public async Task CompletePaymentMethodRegistrationAsync()
    {
        using var activity = Activities.CompletePaymentMethodRegistration(_data.State.ToString());
        await _stateMachine.FireAsync(RegistrationTrigger.CompletePaymentMethodRegistration);
    }

    public async Task CompleteSubscriptionRegistrationAsync()
    {
        using var activity = Activities.CompleteSubscriptionRegistration(_data.State.ToString());
        await _stateMachine.FireAsync(RegistrationTrigger.CompleteSubscriptionRegistration);
    }

    private StateMachine<RegistrationState, RegistrationTrigger> CreateStateMachine(
        RegistrationData data, IStateStore stateStore)
    {
        var stateMachine = new StateMachine<RegistrationState, RegistrationTrigger>(
            () => data.State,
            state =>
            {
                using var activity = Activities.SaveStateMachine(state.ToString());
                
                _data.State = state;
                stateStore.Set(_data.CustomerId.ToString(), _data);
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
        using var activity = Activities.RegisterSubscription(_data.State.ToString());
        
        var request = new RegisterSubscriptionRequest(
            _data.CustomerId,
            _data.SubscriptionDetails.ShippingFrequency,
            _data.SubscriptionDetails.Kind);

        await _subscriptions.RegisterSubscriptionAsync(request);
    }

    private async Task RegisterCustomerDetails()
    {
        using var activity = Activities.RegisterCustomer(_data.State.ToString());
        
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
        using var activity = Activities.RegisterPaymentMethod(_data.State.ToString());

        await _payments.RegisterPaymentMethodAsync(new RegisterPaymentMethodRequest(
            _data.CustomerId,
            _data.PaymentMethodDetails.CardType,
            _data.PaymentMethodDetails.CardNumber,
            _data.PaymentMethodDetails.ExpirationDate,
            _data.PaymentMethodDetails.SecurityCode,
            _data.PaymentMethodDetails.CardHolderName));
    }
}