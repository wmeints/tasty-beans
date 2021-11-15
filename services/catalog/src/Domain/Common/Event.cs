namespace RecommendCoffee.Catalog.Domain.Common;

public abstract record Event
{
    public Guid EventId { get; protected set; }
    public DateTime OccurredOn { get; protected set; }
}