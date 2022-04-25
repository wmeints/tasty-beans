using System.Diagnostics.Metrics;

namespace RecommendCoffee.Registration.Infrastructure;

public class Metrics
{
    private static Meter _meter = new Meter("RecommendCoffee.Registration.Infrastructure");
}