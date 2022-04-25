using RecommendCoffee.Subscriptions.Domain.Services.Shipping.Commands;

namespace RecommendCoffee.Subscriptions.Domain.Services.Shipping;

public interface IShipping
{
    Task CreateShippingOrderAsync(CreateShippingOrderCommand cmd);
}