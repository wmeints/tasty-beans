using System.Diagnostics;
using Microsoft.Extensions.Logging;
using RecommendCoffee.Registration.Application.IntegrationEvents;
using RecommendCoffee.Registration.Domain.Common;
using RecommendCoffee.Registration.Domain.Customers;
using RecommendCoffee.Registration.Domain.Payments;
using RecommendCoffee.Registration.Domain.Registrations;
using RecommendCoffee.Registration.Domain.Subscriptions;

namespace RecommendCoffee.Registration.Application.EventHandlers;

public class SubscriptionStartedEventHandler
{
    private readonly IStateStore _stateStore;
    private readonly ICustomerManagement _customerManagement;
    private readonly ISubscriptions _subscriptions;
    private readonly IPayments _payments;
    private readonly ILogger<SubscriptionStartedEventHandler> _logger;
    
    public SubscriptionStartedEventHandler(
        IStateStore stateStore, 
        ICustomerManagement customerManagement, 
        ISubscriptions subscriptions, 
        IPayments payments,
        ILogger<SubscriptionStartedEventHandler> logger)
    {
        _stateStore = stateStore;
        _customerManagement = customerManagement;
        _subscriptions = subscriptions;
        _logger = logger;
        _payments = payments;
    }

    public async Task HandleAsync(SubscriptionStartedEvent evt)
    {
        _logger.LogInformation("Handling SubscriptionStartedEvent", evt);
        
        var stateData = await _stateStore.Get<RegistrationData>(evt.CustomerId.ToString());
        
        var registration = new Domain.Registrations.Registration(
            stateData, _customerManagement, _subscriptions, _stateStore, _payments);

        await registration.CompleteSubscriptionRegistrationAsync();
    }
}