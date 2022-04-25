using System.Diagnostics.Metrics;

namespace RecommendCoffee.Payments.Domain;

public class Metrics
{
    private static Meter _meter = new Meter("RecommendCoffee.Payments.Domain");
}