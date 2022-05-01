namespace TastyBeans.Shipping.Domain.Aggregates.ShippingOrderAggregate;

public enum OrderStatus
{
    Pending,
    Shipped,
    Delayed,
    FirstDeliveryAttemptFailed,
    SecondDeliveryAttemptFailed,
    Delivered,
    DeliveryFailed
}