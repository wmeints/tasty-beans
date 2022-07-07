using TastyBeans.Simulation.Application.IntegrationEvents;
using TastyBeans.Simulation.Application.Services;

namespace TastyBeans.Simulation.Application.EventHandlers;

public class ShipmentReturnedEventHandler
{
    private readonly ISimulation _simulation;

    public ShipmentReturnedEventHandler(ISimulation simulation)
    {
        _simulation = simulation;
    }

    public async Task HandleAsync(ShipmentReturnedEvent evt)
    {
        if (!await _simulation.IsRunningAsync())
        {
            throw new InvalidOperationException("There's no active simulation. Please start one first");
        }

        await _simulation.ShipmentReturnedAsync(evt.ShippingOrderId);
    }
}