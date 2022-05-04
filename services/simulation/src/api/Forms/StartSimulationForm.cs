using TastyBeans.Simulation.Domain.Aggregates.CustomerAggregate;

namespace TastyBeans.Simulation.Api.Forms;

public class StartSimulationForm
{
    public int CustomerCount { get; set; }
    public List<WeightedCustomerProfile> Profiles { get; set; }
}