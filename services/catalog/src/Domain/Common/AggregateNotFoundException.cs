namespace RecommendCoffee.Catalog.Domain.Common;

public class AggregateNotFoundException : Exception
{
    public AggregateNotFoundException(string message) : base(message)
    {
    }
}