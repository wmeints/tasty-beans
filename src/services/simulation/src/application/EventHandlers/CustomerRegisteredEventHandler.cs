using TastyBeans.Simulation.Application.IntegrationEvents;
using TastyBeans.Simulation.Application.Services;

namespace TastyBeans.Simulation.Application.EventHandlers;

public class CustomerRegisteredEventHandler
{
    private ISimulation _simulation;

    public CustomerRegisteredEventHandler(ISimulation simulation)
    {
        _simulation = simulation;
    }

    public async Task HandleAsync(CustomerRegisteredEvent evt)
    {
        await _simulation.CustomerRegisteredAsync(evt.CustomerId);
    }
}