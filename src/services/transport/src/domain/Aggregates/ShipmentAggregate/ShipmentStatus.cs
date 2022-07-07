namespace TastyBeans.Transport.Domain.Aggregates.ShipmentAggregate;

public enum ShipmentStatus
{
    WaitingForGoods,
    Pending,
    Sorting,
    Sorted,
    OutForDelivery
}