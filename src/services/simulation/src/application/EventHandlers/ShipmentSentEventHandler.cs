using TastyBeans.Simulation.Application.IntegrationEvents;
using TastyBeans.Simulation.Application.Services;

namespace TastyBeans.Simulation.Application.EventHandlers;

public class ShipmentSentEventHandler
{
    private readonly ISimulation _simulation;

    public ShipmentSentEventHandler(ISimulation simulation)
    {
        _simulation = simulation;
    }

    public async Task HandleAsync(ShipmentSentEvent evt)
    {
        if (!await _simulation.IsRunningAsync())
        {
            throw new InvalidOperationException("There's no active simulation. Please start one first");
        }

        await _simulation.ShipmentSentAsync(evt.ShippingOrderId);
    }
}