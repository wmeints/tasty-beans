using Dapr;
using Microsoft.AspNetCore.Mvc;
using RecommendCoffee.Registration.Application.EventHandlers;
using RecommendCoffee.Registration.Application.IntegrationEvents;

namespace RecommendCoffee.Registration.Api.Controllers;

[ApiController]
[Route("/events")]
public class EventsController: ControllerBase
{
    private readonly CustomerRegisteredEventHandler _customerRegisteredEventHandler;
    private readonly SubscriptionStartedEventHandler _subscriptionStartedEventHandler;

    public EventsController(
        CustomerRegisteredEventHandler customerRegisteredEventHandler, 
        SubscriptionStartedEventHandler subscriptionStartedEventHandler)
    {
        _customerRegisteredEventHandler = customerRegisteredEventHandler;
        _subscriptionStartedEventHandler = subscriptionStartedEventHandler;
    }

    [HttpPost("CustomerRegistered")]
    [Topic("pubsub", "customermanagement.customer.registered.v1")]
    public async Task<IActionResult> OnCustomerRegistered(CustomerRegisteredEvent evt)
    {
        await _customerRegisteredEventHandler.HandleAsync(evt);
        return Accepted();
    }

    [HttpPost("SubscriptionStarted")]
    [Topic("pubsub", "subscriptions.subscription.started.v1")]
    public async Task<IActionResult> OnSubscriptionStarted(SubscriptionStartedEvent evt)
    {
        await _subscriptionStartedEventHandler.HandleAsync(evt);
        return Accepted();
    }
}