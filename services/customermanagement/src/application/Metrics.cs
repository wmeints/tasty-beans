using System.Diagnostics.Metrics;

namespace RecommendCoffee.CustomerManagement.Application;

public class Metrics
{
    private static Meter _meter = new Meter("RecommendCoffee.CustomerManagement.Application");
    private static Counter<int> _customerRegistered = _meter.CreateCounter<int>("customermanagement-customers-registered");

    public static Counter<int> CustomerRegistered => _customerRegistered;
}