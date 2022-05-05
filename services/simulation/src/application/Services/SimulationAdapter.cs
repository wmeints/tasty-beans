using Akka.Actor;
using TastyBeans.Simulation.Application.IntegrationEvents;
using TastyBeans.Simulation.Application.Services.Registration;
using TastyBeans.Simulation.Application.Services.Simulation;
using TastyBeans.Simulation.Domain.Aggregates.CustomerAggregate;
using TastyBeans.Simulation.Domain.Aggregates.CustomerAggregate.Commands;
using TastyBeans.Simulation.Domain.Services.Ratings;
using TastyBeans.Simulation.Domain.Services.ShippingInformation;
using TastyBeans.Simulation.Domain.Services.Subscriptions;

namespace TastyBeans.Simulation.Application.Services;

public class SimulationAdapter : ISimulation
{
    private readonly ActorSystem _actorSystem;
    private readonly IActorRef _simulationActor;

    public SimulationAdapter(
        ActorSystem actorSystem, 
        IRegistration registrationService,
        IShippingInformation shippingInformation,
        IRatings ratings,
        ISubscriptions subscriptions)
    {
        _actorSystem = actorSystem;
        
        _simulationActor = actorSystem.ActorOf(
            Simulator.Props(registrationService, shippingInformation, ratings, subscriptions), 
            "simulator");
    }

    public async Task<bool> IsRunningAsync()
    {
        var response = await _simulationActor.Ask(IsSimulationRunning.Instance);

        return response switch
        {
            SimulationStatus(true) => true,
            SimulationStatus(false) => false,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public Task StartSimulationAsync(int customerCount, List<WeightedCustomerProfile> customerProfiles)
    {
        _simulationActor.Tell(new StartSimulation(customerCount, customerProfiles));
        return Task.CompletedTask;
    }

    public async Task DeliveryAttemptFailedAsync(Guid shippingOrderId)
    {
        await _simulationActor.Ask(new DeliveryAttemptFailed(shippingOrderId));
    }

    public async Task DeliveryDelayedAsync(Guid shippingOrderId)
    {
        await _simulationActor.Ask(new DeliveryDelayed(shippingOrderId));
    }

    public async Task DriverOutForDeliveryAsync(Guid shippingOrderId)
    {
        await _simulationActor.Ask(new DriverOutForDelivery(shippingOrderId));
    }

    public async Task ShipmentDeliveredAsync(Guid shippingOrderId)
    {
        await _simulationActor.Ask(new ShipmentDelivered(shippingOrderId));
    }

    public async Task ShipmentLostAsync(Guid shippingOrderId)
    {
        await _simulationActor.Ask(new ShipmentLost(shippingOrderId));
    }

    public async Task ShipmentReturnedAsync(Guid shippingOrderId)
    {
        await _simulationActor.Ask(new ShipmentReturned(shippingOrderId));
    }

    public async Task ShipmentSentAsync(Guid shippingOrderId)
    {
        await _simulationActor.Ask(new ShipmentSent(shippingOrderId));
    }

    public async Task ShipmentSortedAsync(Guid shippingOrderId)
    {
        await _simulationActor.Ask(new ShipmentSorted(shippingOrderId));
    }

    public async Task ShippingOrderCreated(Guid customerId, Guid shippingOrderId)
    {
        await _simulationActor.Ask(new ShippingOrderCreated(customerId, shippingOrderId));
    }

    public async Task CustomerRegisteredAsync(Guid customerId)
    {
        await _simulationActor.Ask(new CustomerRegistered(customerId));
    }
}