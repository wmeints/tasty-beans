namespace TastyBeans.Simulation.Domain.Aggregates.CustomerAggregate;

public class WeightedCustomerProfile
{
    public double Weight { get; set; }
    public CustomerProfile Profile { get; set; }
}