namespace RecommendCoffee.Customers.Domain.Common;

public abstract record Event
{
    public Guid EventId { get; protected set; } = Guid.NewGuid();
    public DateTime OccurredOn { get; protected set; } = DateTime.UtcNow;
}