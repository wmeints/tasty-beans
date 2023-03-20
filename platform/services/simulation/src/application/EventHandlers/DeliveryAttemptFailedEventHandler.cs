using TastyBeans.Simulation.Application.IntegrationEvents;
using TastyBeans.Simulation.Application.Services;

namespace TastyBeans.Simulation.Application.EventHandlers;

public class DeliveryAttemptFailedEventHandler
{
    private ISimulation _simulation;

    public DeliveryAttemptFailedEventHandler(ISimulation simulation)
    {
        _simulation = simulation;
    }

    public async Task HandleAsync(DeliveryAttemptFailedEvent evt)
    {
        if (!await _simulation.IsRunningAsync())
        {
            throw new InvalidOperationException("There's no active simulation. Please start one first");
        }
        
        await _simulation.DeliveryAttemptFailedAsync(evt.ShippingOrderId);
    }
}