using Dapr;
using Microsoft.AspNetCore.Mvc;
using RecommendCoffee.Identity.Application.EventHandlers;
using RecommendCoffee.Identity.Application.IntegrationEvents;

namespace RecommendCoffee.Identity.Api.Controllers;

[ApiController]
[Route("/events")]
public class EventsController: ControllerBase
{
    private readonly CustomerRegisteredEventHandler _customerRegisteredEventHandler;

    [HttpPost("CustomerRegistered")]
    [Topic("pubsub", "customermanagement.customer.registered.v1")]
    public async Task<IActionResult> OnCustomerRegistered(CustomerRegisteredEvent evt)
    {
        await _customerRegisteredEventHandler.HandleAsync(evt);
        return Accepted();
    }
}