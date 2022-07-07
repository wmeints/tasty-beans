using Dapr;
using Microsoft.AspNetCore.Mvc;
using TastyBeans.Shipping.Application.EventHandlers;
using TastyBeans.Shipping.Application.IntegrationEvents;

namespace TastyBeans.Shipping.Api.Controllers;

[ApiController]
[Route("/events")]
public class EventsController :ControllerBase
{
    private readonly ILogger<EventsController> _logger;
    private readonly ProductRegisteredEventHandler _productRegisteredEventHandler;
    private readonly ProductUpdatedEventHandler _productUpdatedEventHandler;
    private readonly CustomerRegisteredEventHandler _customerRegisteredEventHandler;

    public EventsController(
        ILogger<EventsController> logger, 
        ProductRegisteredEventHandler productRegisteredEventHandler, 
        ProductUpdatedEventHandler productUpdatedEventHandler, 
        CustomerRegisteredEventHandler customerRegisteredEventHandler)
    {
        _logger = logger;
        _productRegisteredEventHandler = productRegisteredEventHandler;
        _productUpdatedEventHandler = productUpdatedEventHandler;
        _customerRegisteredEventHandler = customerRegisteredEventHandler;
    }

    [HttpPost("ProductRegistered")]
    [Topic("pubsub", "catalog.product.registered.v1")]
    public async Task<IActionResult> OnProductRegistered(ProductRegisteredEvent evt)
    {
        await _productRegisteredEventHandler.HandleAsync(evt);
        return Accepted();
    }

    [HttpPost("ProductUpdated")]
    [Topic("pubsub", "catalog.product.updated.v1")]
    public async Task<IActionResult> OnProductUpdated(ProductUpdatedEvent evt)
    {
        await _productUpdatedEventHandler.HandleAsync(evt);
        return Accepted();
    }

    [HttpPost("CustomerRegistered")]
    [Topic("pubsub", "customermanagement.customer.registered.v1")]
    public async Task<IActionResult> OnCustomerRegistered(CustomerRegisteredEvent evt)
    {
        await _customerRegisteredEventHandler.HandleAsync(evt);
        return Accepted();
    }
}