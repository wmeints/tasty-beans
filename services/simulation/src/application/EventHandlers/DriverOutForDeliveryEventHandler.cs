using TastyBeans.Simulation.Application.IntegrationEvents;
using TastyBeans.Simulation.Application.Services;

namespace TastyBeans.Simulation.Application.EventHandlers;

public class DriverOutForDeliveryEventHandler
{
    private ISimulation _simulation;

    public DriverOutForDeliveryEventHandler(ISimulation simulation)
    {
        _simulation = simulation;
    }

    public async Task HandleAsync(DriverOutForDeliveryEvent evt)
    {
        if (!await _simulation.IsRunningAsync())
        {
            throw new InvalidOperationException("There's no active simulation. Please start one first");
        }
        
        await _simulation.DriverOutForDeliveryAsync(evt.ShippingOrderId);
    }
}