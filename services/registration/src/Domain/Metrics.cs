using System.Diagnostics.Metrics;

namespace RecommendCoffee.Registration.Domain;

public class Metrics
{
    private static Meter _meter = new Meter("RecommendCoffee.Registration.Domain");
}