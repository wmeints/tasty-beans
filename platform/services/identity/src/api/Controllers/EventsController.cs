using Dapr;
using Microsoft.AspNetCore.Mvc;
using TastyBeans.Identity.Application.EventHandlers;
using TastyBeans.Identity.Application.IntegrationEvents;

namespace TastyBeans.Identity.Api.Controllers;

[ApiController]
[Route("/events")]
public class EventsController: ControllerBase
{
    private readonly CustomerRegisteredEventHandler _customerRegisteredEventHandler;

    public EventsController(CustomerRegisteredEventHandler customerRegisteredEventHandler)
    {
        _customerRegisteredEventHandler = customerRegisteredEventHandler;
    }

    [HttpPost("CustomerRegistered")]
    [Topic("pubsub", "customermanagement.customer.registered.v1")]
    public async Task<IActionResult> OnCustomerRegistered(CustomerRegisteredEvent evt)
    {
        await _customerRegisteredEventHandler.HandleAsync(evt);
        return Accepted();
    }
}