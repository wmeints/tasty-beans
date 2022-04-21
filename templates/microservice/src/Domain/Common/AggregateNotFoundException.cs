namespace RecommendCoffee.Templates.Microservice.Domain.Common;

public class AggregateNotFoundException : Exception
{
    public AggregateNotFoundException(string message) : base(message)
    {
    }
}