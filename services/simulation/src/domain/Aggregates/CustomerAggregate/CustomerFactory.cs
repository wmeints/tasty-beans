using Akka.Actor;

namespace TastyBeans.Simulation.Domain.Aggregates.CustomerAggregate;

public class CustomerFactory
{
    private IActorRefFactory _context;

    public CustomerFactory(IActorRefFactory context)
    {
        _context = context;
    }

    public IActorRef CreateCustomer(Guid customerId)
    {
        return _context.ActorOf(Customer.Props(customerId), $"customer-{customerId}");
    }
}