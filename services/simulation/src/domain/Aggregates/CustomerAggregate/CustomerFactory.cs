using Akka.Actor;
using TastyBeans.Simulation.Domain.Services.ShippingInformation;

namespace TastyBeans.Simulation.Domain.Aggregates.CustomerAggregate;

public class CustomerFactory
{
    private IActorRefFactory _context;
    private readonly IShippingInformation _shippingInformation;
    private Random _random = new();
    private readonly List<(double, WeightedCustomerProfile)> _customerProfiles = new();
    private readonly double _cumulativeWeight;

    public CustomerFactory(
        IActorRefFactory context,
        IShippingInformation shippingInformation,
        List<WeightedCustomerProfile> customerProfiles)
    {
        _context = context;
        _shippingInformation = shippingInformation;

        _cumulativeWeight = 0.0;
        
        for (int index = 0; index < customerProfiles.Count; index++)
        {
            _cumulativeWeight += customerProfiles[index].Weight;
            _customerProfiles.Add((_cumulativeWeight, customerProfiles[index]));
        }
    }

    public IActorRef CreateCustomer(Guid customerId)
    {
        double expectedWeight = _random.NextDouble() * _cumulativeWeight;
        CustomerProfile? selectedCustomerProfile = null;

        foreach (var (cumulativeWeight, customerProfile) in _customerProfiles)
        {
            if (cumulativeWeight >= expectedWeight)
            {
                selectedCustomerProfile = customerProfile.Profile;
                break;
            }
        }

        if (selectedCustomerProfile == null)
        {
            throw new InvalidOperationException("Can't find a fitting customer profile");
        }
        
        return _context.ActorOf(Customer.Props(customerId, selectedCustomerProfile, _shippingInformation), $"customer-{customerId}");
    }
}