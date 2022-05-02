using Akka.Actor;
using TastyBeans.Simulation.Domain.Aggregates.CustomerAggregate;
using TastyBeans.Simulation.Domain.Aggregates.CustomerAggregate.Commands;

namespace TastyBeans.Simulation.Application.Services.Simulation;

public class Simulator: ReceiveActor
{
    private readonly Dictionary<Guid, IActorRef> _customersById = new();
    private readonly Dictionary<Guid, IActorRef> _customersByShippingOrderId = new();
    private readonly CustomerFactory _customerFactory;
    
    public Simulator()
    {
        _customerFactory = new CustomerFactory(Context);
        
        // When a new order is received, we need to connect the order to the customer.
        // Otherwise there's no way we can properly route order related events to a customer.
        
        Receive<ShippingOrderCreated>(msg => _customersByShippingOrderId.Add(msg.ShippingOrderId, LocateCustomerById(msg.CustomerId)));
        Receive<CustomerRegistered>(msg =>
        {
            var customerRef = _customerFactory.CreateCustomer(msg.CustomerId);
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
    }

    private IActorRef LocateCustomerById(Guid customerId)
    {
        if (!_customersById.TryGetValue(customerId, out var customerRef))
        {
            customerRef = ActorRefs.Nobody;
        }

        return customerRef;
    }

    public static Props Props()
    {
        return new Props(
            typeof(Simulator), 
            Akka.Actor.SupervisorStrategy.DefaultStrategy);
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