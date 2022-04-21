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

    public ShippingOrder(Guid id, Guid customerId, string firstName, string lastName, string street, string houseNumber,
        string postalCode, string city, string countryCode, ICollection<OrderItem> orderItems)
    {
        Id = id;
        CustomerId = customerId;
        FirstName = firstName;
        LastName = lastName;
        Street = street;
        HouseNumber = houseNumber;
        PostalCode = postalCode;
        City = city;
        CountryCode = countryCode;
        OrderItems = orderItems;
        Status = OrderStatus.Pending;
        OrderDate = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }
    public Guid CustomerId { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Street { get; private set; }
    public string HouseNumber { get; private set; }
    public string PostalCode { get; private set; }
    public string City { get; private set; }
    public string CountryCode { get; private set; }
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

        var order = new ShippingOrder(
            Guid.NewGuid(), cmd.CustomerId, cmd.FirstName, cmd.LastName, cmd.Street,
            cmd.HouseNumber, cmd.PostalCode, cmd.City, cmd.CountryCode, cmd.OrderItems.ToList());

        var shippingOrderCreatedEvent = new ShippingOrderCreatedEvent(order);

        return new CreateShippingOrderCommandResponse(
            order, Enumerable.Empty<ValidationError>(),
            new[] {shippingOrderCreatedEvent});
    }
}