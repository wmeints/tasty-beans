namespace TastyBeans.Subscriptions.Domain.Services.Shipping.Commands;

public record CreateShippingOrderCommand(Guid CustomerId, Guid ProductId);