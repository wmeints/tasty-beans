using Dapr;
using Microsoft.AspNetCore.Mvc;
using RecommendCoffee.Ratings.Application.EventHandlers;
using RecommendCoffee.Ratings.Application.IntegrationEvents;

namespace RecommendCoffee.Ratings.Api.Controllers;

[ApiController]
[Route("/events")]
public class EventsController :ControllerBase
{
    private readonly ILogger<EventsController> _logger;
    private readonly ProductRegisteredEventHandler _productRegisteredEventHandler;
    private readonly ProductUpdatedEventHandler _productUpdatedEventHandler;
    private readonly ProductDiscontinuedEventHandler _productDiscontinuedEventHandler;
    private readonly CustomerRegisteredEventHandler _customerRegisteredEventHandler;

    public EventsController(
        ILogger<EventsController> logger, 
        ProductRegisteredEventHandler productRegisteredEventHandler, 
        ProductUpdatedEventHandler productUpdatedEventHandler, 
        ProductDiscontinuedEventHandler productDiscontinuedEventHandler, 
        CustomerRegisteredEventHandler customerRegisteredEventHandler)
    {
        _logger = logger;
        _productRegisteredEventHandler = productRegisteredEventHandler;
        _productUpdatedEventHandler = productUpdatedEventHandler;
        _productDiscontinuedEventHandler = productDiscontinuedEventHandler;
        _customerRegisteredEventHandler = customerRegisteredEventHandler;
    }

    [HttpPost("ProductRegistered")]
    [Topic("pubsub", "catalog.product.registered.v1")]
    public async Task<IActionResult> OnProductRegistered(ProductRegisteredEvent evt)
    {
        _logger.HandlingProductRegisteredEvent();
        await _productRegisteredEventHandler.HandleAsync(evt);
        return Accepted();
    }

    [HttpPost("ProductUpdated")]
    [Topic("pubsub", "catalog.product.updated.v1")]
    public async Task<IActionResult> OnProductUpdated(ProductUpdatedEvent evt)
    {
        _logger.HandlingProductUpdatedEvent();
        await _productUpdatedEventHandler.HandleAsync(evt);
        return Accepted();
    }

    [HttpPost("ProductDiscontinued")]
    [Topic("pubsub", "catalog.product.discontinued.v1")]
    public async Task<IActionResult> OnProductDiscontinued(ProductDiscontinuedEvent evt)
    {
        _logger.HandlingProductDiscontinuedEvent();
        await _productDiscontinuedEventHandler.HandleAsync(evt);
        return Accepted();
    }

    [HttpPost("CustomerRegistered")]
    [Topic("pubsub", "customermanagement.customer.registered.v1")]
    public async Task<IActionResult> OnCustomerRegistered(CustomerRegisteredEvent evt)
    {
        _logger.HandlingCustomerRegisteredEvent();
        await _customerRegisteredEventHandler.HandleAsync(evt);
        return Accepted();
    }
}