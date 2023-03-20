using TastyBeans.Subscriptions.Domain.Services.Shipping.Commands;

namespace TastyBeans.Subscriptions.Domain.Services.Shipping;

public interface IShipping
{
    Task CreateShippingOrderAsync(CreateShippingOrderCommand cmd);
}