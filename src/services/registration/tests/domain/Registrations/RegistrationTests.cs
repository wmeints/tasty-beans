using System;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using TastyBeans.Registration.Domain.Customers;
using TastyBeans.Registration.Domain.Payments;
using TastyBeans.Registration.Domain.Registrations;
using TastyBeans.Registration.Domain.Registrations.Commands;
using TastyBeans.Registration.Domain.Subscriptions;
using TastyBeans.Shared.Domain;
using Xunit;

namespace TastyBeans.Registration.Domain.Tests.Registrations;

public class RegistrationTests
{
    private readonly IStateStore _stateStore;
    private readonly CustomerDetails _customerDetails;
    private readonly ICustomerManagement _customerManagement;
    private readonly ISubscriptions _subscriptions;
    private readonly SubscriptionDetails _subscriptionDetails;
    private readonly PaymentMethodDetails _paymentMethodDetails;
    private readonly IPayments _payments;
    
    public RegistrationTests()
    {
        _stateStore = A.Fake<IStateStore>();
        _customerManagement = A.Fake<ICustomerManagement>();
        _subscriptions = A.Fake<ISubscriptions>();
        _payments = A.Fake<IPayments>();
        
        _customerDetails = new CustomerDetails(
            "Willem",
            "Meints",
            "test@domain.org",
            "00-00000000",
            new Address("Test", "Test", "0000 AA", "Test", "NL"),
            new Address("Test", "Test", "0000 AA", "Test", "NL")
        );

        _subscriptionDetails = new SubscriptionDetails(ShippingFrequency.Monthly, SubscriptionKind.OneYear);
        _paymentMethodDetails = new PaymentMethodDetails(
            CardType.Mastercard,
            "5413675197898462",
            "01/22",
            "679", 
            "Test User");
    }
    
    [Fact]
    public async Task CanRegisterCustomerDetails()
    {
        var registration = new Domain.Registrations.Registration(
            _customerManagement, _subscriptions, _stateStore, _payments);

        await registration.StartAsync(new StartRegistrationCommand(
            Guid.NewGuid(),
            _customerDetails,
            _subscriptionDetails,
            _paymentMethodDetails));

        A.CallTo(() => _customerManagement.RegisterCustomerAsync(
            A<RegisterCustomerRequest>.Ignored)).MustHaveHappened();
    }

    [Fact]
    public async Task CanCompleteCustomerRegistration()
    {
        var registrationData = new RegistrationData
        {
            State = RegistrationState.WaitingForCustomerRegistration,
            CustomerDetails = _customerDetails,
            CustomerId = Guid.NewGuid(),
            SubscriptionDetails = _subscriptionDetails,
            PaymentMethodDetails = _paymentMethodDetails
        };
        
        var registration = new Domain.Registrations.Registration(
            registrationData, _customerManagement, _subscriptions, _stateStore, _payments);
        
        await registration.CompleteCustomerRegistrationAsync();

        registrationData.State.Should().Be(RegistrationState.WaitingForPaymentMethodRegistration);
    }

    [Fact]
    public async Task CanCompletePaymentMethodRegistration()
    {
        var registrationData = new RegistrationData
        {
            State = RegistrationState.WaitingForPaymentMethodRegistration,
            CustomerDetails = _customerDetails,
            CustomerId = Guid.NewGuid(),
            SubscriptionDetails = _subscriptionDetails,
            PaymentMethodDetails = _paymentMethodDetails
        };
        
        var registration = new Domain.Registrations.Registration(
            registrationData, _customerManagement, _subscriptions, _stateStore, _payments);

        await registration.CompletePaymentMethodRegistrationAsync();

        registrationData.State.Should().Be(RegistrationState.WaitingForSubscriptionRegistration);
    }

    [Fact]
    public async Task CanCompleteSubscriptionDetailsRegistration()
    {
        var registrationData = new RegistrationData
        {
            State = RegistrationState.WaitingForSubscriptionRegistration,
            CustomerDetails = _customerDetails,
            CustomerId = Guid.NewGuid(),
            SubscriptionDetails = _subscriptionDetails,
            PaymentMethodDetails = _paymentMethodDetails
        };
        
        var registration = new Domain.Registrations.Registration(
            registrationData, _customerManagement, _subscriptions, _stateStore, _payments);

        await registration.CompleteSubscriptionRegistrationAsync();

        registrationData.State.Should().Be(RegistrationState.Completed);
    }
}