namespace TastyBeans.Simulation.Domain.Services.ShippingInformation;

public interface IShippingInformation
{
    Task<ShippingOrder> GetShippingOrderAsync(Guid shippingOrderId);
}