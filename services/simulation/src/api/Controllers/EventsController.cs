using System.Text.Json;
using Dapr;
using Microsoft.AspNetCore.Mvc;
using TastyBeans.Simulation.Application.EventHandlers;
using TastyBeans.Simulation.Application.IntegrationEvents;

namespace TastyBeans.Simulation.Api.Controllers;

[ApiController]
[Route("/events")]
public class EventsController : ControllerBase
{
    private readonly ShipmentDeliveredEventHandler _shipmentDeliveredEventHandler;
    private readonly ShipmentLostEventHandler _shipmentLostEventHandler;
    private readonly ShipmentSentEventHandler _shipmentSentEventHandler;
    private readonly DeliveryDelayedEventHandler _deliveryDelayedEventHandler;
    private readonly ShipmentSortedEventHandler _shipmentSortedEventHandler;
    private readonly ShipmentReturnedEventHandler _shipmentReturnedEventHandler;
    private readonly DriverOutForDeliveryEventHandler _driverOutForDeliveryEventHandler;
    private readonly CustomerRegisteredEventHandler _customerRegisteredEventHandler;

    private readonly ILogger<EventsController> _logger;

    public EventsController(
        ShipmentDeliveredEventHandler shipmentDeliveredEventHandler,
        ShipmentLostEventHandler shipmentLostEventHandler,
        ShipmentSentEventHandler shipmentSentEventHandler,
        DeliveryDelayedEventHandler deliveryDelayedEventHandler,
        ShipmentSortedEventHandler shipmentSortedEventHandler,
        ShipmentReturnedEventHandler shipmentReturnedEventHandler,
        DriverOutForDeliveryEventHandler driverOutForDeliveryEventHandler,
        CustomerRegisteredEventHandler customerRegisteredEventHandler,
        ILogger<EventsController> logger)
    {
        _shipmentDeliveredEventHandler = shipmentDeliveredEventHandler;
        _shipmentLostEventHandler = shipmentLostEventHandler;
        _shipmentSentEventHandler = shipmentSentEventHandler;
        _deliveryDelayedEventHandler = deliveryDelayedEventHandler;
        _shipmentSortedEventHandler = shipmentSortedEventHandler;
        _shipmentReturnedEventHandler = shipmentReturnedEventHandler;
        _driverOutForDeliveryEventHandler = driverOutForDeliveryEventHandler;
        _customerRegisteredEventHandler = customerRegisteredEventHandler;
        _logger = logger;
    }

    [HttpPost("CustomerRegistered")]
    [Topic("pubsub", "customermanagement.customer.registered.v1")]
    public async Task<IActionResult> CustomerRegistered(CustomerRegisteredEvent evt)
    {
        await _customerRegisteredEventHandler.HandleAsync(evt);
        return Accepted();
    }


    [HttpPost("ShipmentDelivered")]
    [Topic("pubsub", "transport.shipment.delivered.v1")]
    public async Task<IActionResult> ShipmentDelivered(ShipmentDeliveredEvent evt)
    {
        await _shipmentDeliveredEventHandler.HandleAsync(evt);
        return Accepted();
    }

    [HttpPost("ShipmentLost")]
    [Topic("pubsub", "transport.shipment.lost.v1")]
    public async Task<IActionResult> ShipmentLost(ShipmentLostEvent evt)
    {
        await _shipmentLostEventHandler.HandleAsync(evt);
        return Accepted();
    }

    [HttpPost("ShipmentSent")]
    [Topic("pubsub", "transport.shipment.sent.v1")]
    public async Task<IActionResult> ShipmentSent(ShipmentSentEvent evt)
    {
        await _shipmentSentEventHandler.HandleAsync(evt);
        return Accepted();
    }

    [HttpPost("DeliveryDelayed")]
    [Topic("pubsub", "transport.shipment.delayed.v1")]
    public async Task<IActionResult> DeliveryDelayed(DeliveryDelayedEvent evt)
    {
        await _deliveryDelayedEventHandler.HandleAsync(evt);
        return Accepted();
    }

    [HttpPost("ShipmentSorted")]
    [Topic("pubsub", "transport.shipment.sorted.v1")]
    public async Task<IActionResult> ShipmentSorted(ShipmentSortedEvent evt)
    {
        await _shipmentSortedEventHandler.HandleAsync(evt);
        return Accepted();
    }

    [HttpPost("ShipmentReturned")]
    [Topic("pubsub", "transport.shipment.returned.v1")]
    public async Task<IActionResult> ShipmentReturned(ShipmentReturnedEvent evt)
    {
        await _shipmentReturnedEventHandler.HandleAsync(evt);
        return Accepted();
    }

    [HttpPost("DriverOutForDelivery")]
    [Topic("pubsub", "transport.shipment.driver-out-for-delivery.v1")]
    public async Task<IActionResult> DriverOutForDelivery(DriverOutForDeliveryEvent evt)
    {
        await _driverOutForDeliveryEventHandler.HandleAsync(evt);
        return Accepted();
    }
}