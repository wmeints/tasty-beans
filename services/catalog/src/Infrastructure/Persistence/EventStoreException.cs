namespace RecommendCoffee.Catalog.Infrastructure.Persistence;

public class EventStoreException : Exception
{
    public EventStoreException(string message) : base(message)
    {

    }
}
