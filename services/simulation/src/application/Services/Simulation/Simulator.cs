using Akka.Actor;
using TastyBeans.Simulation.Application.Services.Registration;
using TastyBeans.Simulation.Domain.Aggregates.CustomerAggregate;
using TastyBeans.Simulation.Domain.Aggregates.CustomerAggregate.Commands;

namespace TastyBeans.Simulation.Application.Services.Simulation;

public class Simulator: ReceiveActor
{
    private readonly Dictionary<Guid, IActorRef> _customersById = new();
    private readonly Dictionary<Guid, IActorRef> _customersByShippingOrderId = new();
    private readonly CustomerFactory _customerFactory;
    private readonly RegistrationDataFactory _registrationDataFactory;
    private readonly IRegistration _registrationService;
    private bool _running;
    
    public Simulator(IRegistration registrationService)
    {
        _registrationService = registrationService;
        _customerFactory = new CustomerFactory(Context);
        _registrationDataFactory = new RegistrationDataFactory();
        
        // When a new order is received, we need to connect the order to the customer.
        // Otherwise there's no way we can properly route order related events to a customer.
        
        Receive<ShippingOrderCreated>(msg => _customersByShippingOrderId.Add(
            msg.ShippingOrderId, LocateCustomerById(msg.CustomerId)));
        
        // When a customer is registered we connect behavior to that customer.
        
        Receive<CustomerRegistered>(msg =>
        {
            var customerRef = _customerFactory.CreateCustomer(msg.CustomerId);
            _customersById.Add(msg.CustomerId, customerRef);
        });
        
        // Route incoming events to the correct customer actor in the system.
        // The customer actor will perform actions based on what is happening to their orders.
        
        Receive<DeliveryDelayed>(msg => LocateCustomerByShippingOrder(msg.ShippingOrderId).Tell(msg));
        Receive<DeliveryAttemptFailed>(msg => LocateCustomerByShippingOrder(msg.ShippingOrderId).Tell(msg));
        Receive<DriverOutForDelivery>(msg => LocateCustomerByShippingOrder(msg.ShippingOrderId).Tell(msg));
        Receive<ShipmentSent>(msg => LocateCustomerByShippingOrder(msg.ShippingOrderId).Tell(msg));
        Receive<ShipmentSorted>(msg => LocateCustomerByShippingOrder(msg.ShippingOrderId).Tell(msg));

        // There are several signals that will result in an order reaching a finalized state.
        // When this happens, we need to perform some house keeping as well as handling the message itself.
        
        Receive<ShipmentDelivered>(msg =>
        {
            LocateCustomerByShippingOrder(msg.ShippingOrderId).Tell(msg);
            _customersByShippingOrderId.Remove(msg.ShippingOrderId);
        });
        
        Receive<ShipmentReturned>(msg =>
        {
            LocateCustomerByShippingOrder(msg.ShippingOrderId).Tell(msg);
            _customersByShippingOrderId.Remove(msg.ShippingOrderId);
        });
        
        Receive<ShipmentLost>(msg =>
        {
            LocateCustomerByShippingOrder(msg.ShippingOrderId).Tell(msg);
            _customersByShippingOrderId.Remove(msg.ShippingOrderId);
        });

        Receive<StartSimulation>(msg => OnStartSimulation(msg.CustomerCount));
        Receive<IsSimulationRunning>(msg => Sender.Tell(new SimulationStatus(_running)));
        
        // The start simulation operation will fire n requests to register customers in the system.
        // These will come back as messages to the simulator which will have to call the IRegistration service.
        
        Receive<RegisterCustomer>(msg => OnRegisterCustomer());
    }

    public static Props Props(IRegistration registrationService)
    {
        return new Props(
            type: typeof(Simulator),
            supervisorStrategy: Akka.Actor.SupervisorStrategy.DefaultStrategy,
            args: registrationService);
    }
    
    private void OnRegisterCustomer()
    {
        var request = _registrationDataFactory.Create();
        
        var requestHandler = Context.ActorOf(
            CustomerRegistrationWorker.Props(_registrationService));
        
        requestHandler.Tell(request);
    }

    private void OnStartSimulation(int customerCount)
    {
        _running = true;
        
        for (int i = 0; i < customerCount; i++)
        {
            Self.Tell(RegisterCustomer.Instance);
        }
    }

    private IActorRef LocateCustomerById(Guid customerId)
    {
        if (!_customersById.TryGetValue(customerId, out var customerRef))
        {
            customerRef = ActorRefs.Nobody;
        }

        return customerRef;
    }

    private IActorRef LocateCustomerByShippingOrder(Guid shippingOrderId)
    {
        if (!_customersByShippingOrderId.TryGetValue(shippingOrderId, out var customerRef))
        {
            customerRef = ActorRefs.Nobody;
        }

        return customerRef;
    }
}