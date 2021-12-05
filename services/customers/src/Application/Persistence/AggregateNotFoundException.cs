using RecommendCoffee.Customers.Domain.Common;

namespace RecommendCoffee.Customers.Application.Persistence;

public class AggregateNotFoundException : Exception
{
    private AggregateNotFoundException(string message) : base(message)
    {

    }

    public static AggregateNotFoundException Create<TAggregate, TKey>(TKey aggregateId) where TAggregate : AggregateRoot<TKey>
    {
        var message = $"Could not find {typeof(TAggregate).Name} with key {aggregateId}";
        return new AggregateNotFoundException(message);
    }
}
