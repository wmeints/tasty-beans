namespace TastyBeans.Shared.Domain;

public class AggregateNotFoundException : Exception
{
    public AggregateNotFoundException(string message) : base(message)
    {
    }
}