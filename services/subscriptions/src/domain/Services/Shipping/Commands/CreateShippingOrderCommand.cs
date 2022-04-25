namespace RecommendCoffee.Subscriptions.Domain.Services.Shipping.Commands;

public record CreateShippingOrderCommand(Guid CustomerId, Guid ProductId);