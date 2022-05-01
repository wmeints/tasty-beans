using System.Diagnostics.Metrics;

namespace TastyBeans.Catalog.Application;

public static class Metrics
{
    private static readonly Meter Meter = new Meter("TastyBeans.Catalog.Application");
    
    public static readonly Counter<int> ProductsRegistered = Meter.CreateCounter<int>("catalog-products-registered");
    public static readonly Counter<int> ProductsDiscontinued = Meter.CreateCounter<int>("catalog-products-discontinued");
    public static readonly Counter<int> ProductsTasteTested = Meter.CreateCounter<int>("catalog-products-tastetested");
}