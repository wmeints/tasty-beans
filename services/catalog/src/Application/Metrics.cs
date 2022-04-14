using System.Diagnostics.Metrics;

namespace RecommendCoffee.Catalog.Application;

public class Metrics
{
    private static Meter _meter = new Meter("RecommendCoffee.Catalog.Application");
    private static Counter<int> _productsRegistered = _meter.CreateCounter<int>("catalog-products-registered");
    private static Counter<int> _productsDiscontinued = _meter.CreateCounter<int>("catalog-products-discontinued");
    private static Counter<int> _productsTasteTested = _meter.CreateCounter<int>("catalog-products-tastetested");
    
    public static Counter<int> ProductsRegistered => _productsRegistered;
    public static Counter<int> ProductsDiscontinued => _productsDiscontinued;
    public static Counter<int> ProductsTasteTested => _productsTasteTested;
}