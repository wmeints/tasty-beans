using TastyBeans.Simulation.Application.IntegrationEvents;
using TastyBeans.Simulation.Application.Services;

namespace TastyBeans.Simulation.Application.EventHandlers;

public class ShippingOrderCreatedEventHandler
{
    private ISimulation _simulation;

    public ShippingOrderCreatedEventHandler(ISimulation simulation)
    {
        _simulation = simulation;
    }

    public async Task HandleAsync(ShippingOrderCreatedEvent evt)
    {
        if (!await _simulation.IsRunningAsync())
        {
            throw new InvalidOperationException("There's no active simulation. Please start one first");
        }

        await _simulation.ShippingOrderCreated(evt.CustomerId, evt.ShippingOrderId);
    }
}