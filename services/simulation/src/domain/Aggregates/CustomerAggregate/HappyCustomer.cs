using Akka.Actor;

namespace TastyBeans.Simulation.Domain.Aggregates.CustomerAggregate;

public class HappyCustomer: ReceiveActor
{
    private readonly Guid _customerId;

    public HappyCustomer(Guid customerId)
    {
        _customerId = customerId;
    }
    
    public static Props Props(Guid customerId)
    {
        return new Props(typeof(HappyCustomer), Akka.Actor.SupervisorStrategy.DefaultStrategy, customerId);
    }
}