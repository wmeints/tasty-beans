using System.Diagnostics.Metrics;

namespace RecommendCoffee.CustomerManagement.Domain;

public class Metrics
{
    private static Meter _meter = new Meter("RecommendCoffee.CustomerManagement.Domain");
}