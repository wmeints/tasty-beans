namespace RecommendCoffee.Customers.Infrastructure.Persistence;

public class EventStoreException : Exception
{
    public EventStoreException(string message) : base(message)
    {

    }
}
