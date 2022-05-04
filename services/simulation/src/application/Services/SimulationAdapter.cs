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

    public Task DeliveryAttemptFailedAsync(Guid shippingOrderId)
    {
        _simulationActor.Tell(new DeliveryAttemptFailed(shippingOrderId));
        return Task.CompletedTask;
    }

    public Task DeliveryDelayedAsync(Guid shippingOrderId)
    {
        _simulationActor.Tell(new DeliveryDelayed(shippingOrderId));
        return Task.CompletedTask;
    }

    public Task DriverOutForDeliveryAsync(Guid shippingOrderId)
    {
        _simulationActor.Tell(new DriverOutForDelivery(shippingOrderId));
        return Task.CompletedTask;
    }

    public Task ShipmentDeliveredAsync(Guid shippingOrderId)
    {
        _simulationActor.Tell(new ShipmentDelivered(shippingOrderId));
        return Task.CompletedTask;
    }

    public Task ShipmentLostAsync(Guid shippingOrderId)
    {
        _simulationActor.Tell(new ShipmentLost(shippingOrderId));
        return Task.CompletedTask;
    }

    public Task ShipmentReturnedAsync(Guid shippingOrderId)
    {
        _simulationActor.Tell(new ShipmentReturned(shippingOrderId));
        return Task.CompletedTask;
    }

    public Task ShipmentSentAsync(Guid shippingOrderId)
    {
        _simulationActor.Tell(new ShipmentSent(shippingOrderId));
        return Task.CompletedTask;
    }

    public Task ShipmentSortedAsync(Guid shippingOrderId)
    {
        _simulationActor.Tell(new ShipmentSorted(shippingOrderId));
        return Task.CompletedTask;
    }

    public Task ShippingOrderCreated(Guid customerId, Guid shippingOrderId)
    {
        _simulationActor.Tell(new ShippingOrderCreated(customerId, shippingOrderId));
        return Task.CompletedTask;
    }

    public Task CustomerRegisteredAsync(Guid customerId)
    {
        _simulationActor.Tell(new CustomerRegistered(customerId));
        return Task.CompletedTask;
    }
}