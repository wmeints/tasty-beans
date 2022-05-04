using TastyBeans.Simulation.Application.IntegrationEvents;
using TastyBeans.Simulation.Application.Services;

namespace TastyBeans.Simulation.Application.EventHandlers;

public class DeliveryDelayedEventHandler
{
    private ISimulation _simulation;

    public DeliveryDelayedEventHandler(ISimulation simulation)
    {
        _simulation = simulation;
    }

    public async Task HandleAsync(DeliveryDelayedEvent evt)
    {
        if (!await _simulation.IsRunningAsync())
        {
            throw new InvalidOperationException("There's no active simulation. Please start one first");
        }
        
        await _simulation.DeliveryDelayedAsync(evt.ShippingOrderId);
    }
}