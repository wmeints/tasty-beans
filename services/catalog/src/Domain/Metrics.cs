using System.Diagnostics.Metrics;

namespace RecommendCoffee.Catalog.Domain;

public class Metrics
{
    private static Meter Meter = new Meter("RecommendCoffee.Catalog.Domain");
}