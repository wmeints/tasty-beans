using System.Diagnostics.Metrics;

namespace RecommendCoffee.Catalog.Domain;

public class Metrics
{
    private static Meter _meter = new Meter("RecommendCoffee.Catalog.Domain");
}