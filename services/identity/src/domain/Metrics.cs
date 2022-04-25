using System.Diagnostics.Metrics;

namespace RecommendCoffee.Identity.Domain;

public class Metrics
{
    private static Meter _meter = new Meter("RecommendCoffee.Identity.Domain");
}