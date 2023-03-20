using Dapr;
using Microsoft.AspNetCore.Mvc;
using TastyBeans.Subscriptions.Api.Services;
using TastyBeans.Subscriptions.Application.IntegrationEvents;

namespace TastyBeans.Subscriptions.Api.Controllers;

[ApiController]
[Route("/events")]
public class EventsController: ControllerBase
{
    private readonly BackgroundTaskQueue _backgroundTaskQueue;
    
    public EventsController(BackgroundTaskQueue backgroundTaskQueue)
    {
        _backgroundTaskQueue = backgroundTaskQueue;
    }

    [HttpPost("MonthHasPassed")]
    [Topic("pubsub", "timer.month-has-passed.v1")]
    public async Task<IActionResult> OnMonthHasPassed(MonthHasPassedEvent evt)
    {
        // Queue the event on the background worker for processing.
        await _backgroundTaskQueue.Enqueue(evt);
        
        return Accepted();
    }
}