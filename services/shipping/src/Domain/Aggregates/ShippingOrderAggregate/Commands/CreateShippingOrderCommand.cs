namespace RecommendCoffee.Shipping.Domain.Aggregates.ShippingOrderAggregate.Commands;

public record CreateShippingOrderCommand(
    Guid CustomerId,
    string FirstName,
    string LastName,
     string Street,
    string HouseNumber,
    string PostalCode,
    string City,
    string CountryCode,
    IEnumerable<OrderItem> OrderItems);