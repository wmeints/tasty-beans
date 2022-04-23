using System.Collections.ObjectModel;
using RecommendCoffee.Ratings.Domain.Common;
using RecommendCoffee.Shipping.Domain.Aggregates.ShippingOrderAggregate.Commands;
using RecommendCoffee.Shipping.Domain.Aggregates.ShippingOrderAggregate.Events;
using RecommendCoffee.Shipping.Domain.Aggregates.ShippingOrderAggregate.Validators;
using RecommendCoffee.Shipping.Domain.Common;

namespace RecommendCoffee.Shipping.Domain.Aggregates.ShippingOrderAggregate;

public class ShippingOrder
{
    private ShippingOrder()
    {
        OrderItems = new Collection<OrderItem>();
    }

    public ShippingOrder(Guid id, Guid customerId, ICollection<OrderItem> orderItems)
    {
        Id = id;
        CustomerId = customerId;
        OrderItems = orderItems;
        Status = OrderStatus.Pending;
        OrderDate = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }
    public Guid CustomerId { get; private set; }
    public DateTime OrderDate { get; private set; }
    public DateTime? LastUpdatedDate { get; private set; }
    public ICollection<OrderItem> OrderItems { get; private set; }
    public OrderStatus Status { get; private set; }

    public static CreateShippingOrderCommandResponse Create(CreateShippingOrderCommand cmd)
    {
        var validator = new CreateShippingOrderCommandValidator();
        var validationResult = validator.Validate(cmd);

        if (!validationResult.IsValid)
        {
            return new CreateShippingOrderCommandResponse(
                null,
                validationResult.GetValidationErrors(),
                Enumerable.Empty<IDomainEvent>());
        }

        var order = new ShippingOrder(Guid.NewGuid(), cmd.CustomerId, cmd.OrderItems.ToList());
        var shippingOrderCreatedEvent = new ShippingOrderCreatedEvent(order);

        return new CreateShippingOrderCommandResponse(
            order, Enumerable.Empty<ValidationError>(),
            new[] {shippingOrderCreatedEvent});
    }
}