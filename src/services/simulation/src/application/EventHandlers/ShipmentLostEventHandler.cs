using TastyBeans.Simulation.Application.IntegrationEvents;
using TastyBeans.Simulation.Application.Services;

namespace TastyBeans.Simulation.Application.EventHandlers;

public class ShipmentLostEventHandler
{
    private readonly ISimulation _simulation;

    public ShipmentLostEventHandler(ISimulation simulation)
    {
        _simulation = simulation;
    }

    public async Task HandleAsync(ShipmentLostEvent evt)
    {
        if (!await _simulation.IsRunningAsync())
        {
            throw new InvalidOperationException("There's no active simulation. Please start one first");
        }

        await _simulation.ShipmentLostAsync(evt.ShippingOrderId);
    }
}