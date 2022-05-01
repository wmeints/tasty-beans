using Dapr;
using Microsoft.AspNetCore.Mvc;
using TastyBeans.Registration.Application.EventHandlers;
using TastyBeans.Registration.Application.IntegrationEvents;

namespace TastyBeans.Registration.Api.Controllers;

[ApiController]
[Route("/events")]
public class EventsController: ControllerBase
{
    private readonly ILogger<EventsController> _logger;
    private readonly CustomerRegisteredEventHandler _customerRegisteredEventHandler;
    private readonly SubscriptionStartedEventHandler _subscriptionStartedEventHandler;
    private readonly PaymentMethodRegisteredEventHandler _paymentMethodRegisteredEventHandler;
    
    public EventsController(
        CustomerRegisteredEventHandler customerRegisteredEventHandler, 
        SubscriptionStartedEventHandler subscriptionStartedEventHandler,
        PaymentMethodRegisteredEventHandler paymentMethodRegisteredEventHandler,
        ILogger<EventsController> logger)
    {
        _customerRegisteredEventHandler = customerRegisteredEventHandler;
        _subscriptionStartedEventHandler = subscriptionStartedEventHandler;
        _paymentMethodRegisteredEventHandler = paymentMethodRegisteredEventHandler;
        _logger = logger;
    }

    [HttpPost("CustomerRegistered")]
    [Topic("pubsub", "customermanagement.customer.registered.v1")]
    public async Task<IActionResult> OnCustomerRegistered(CustomerRegisteredEvent evt)
    {
        _logger.LogInformation("Handling CustomerRegistered.V1 event");
        
        await _customerRegisteredEventHandler.HandleAsync(evt);
        return Accepted();
    }

    [HttpPost("SubscriptionStarted")]
    [Topic("pubsub", "subscriptions.subscription.started.v1")]
    public async Task<IActionResult> OnSubscriptionStarted(SubscriptionStartedEvent evt)
    {
        _logger.LogInformation("Handling SubscriptionStarted.V1 event");
        
        await _subscriptionStartedEventHandler.HandleAsync(evt);
        return Accepted();
    }

    [HttpPost("PaymentMethodRegistered")]
    [Topic("pubsub", "payments.paymentmethod.registered.v1")]
    public async Task<IActionResult> OnPaymentMethodRegistered(PaymentMethodRegisteredEvent evt)
    {
        _logger.LogInformation("Handling PaymentMethodRegistered.V1 event");

        await _paymentMethodRegisteredEventHandler.HandleAsync(evt);
        return Accepted();
    }
}