using TastyBeans.Shipping.Domain.Aggregates.ShippingOrderAggregate;

namespace TastyBeans.Shipping.Application.QueryHandlers;

public class FindShippingOrderQueryHandler
{
    private readonly IShippingOrderRepository _shippingOrderRepository;

    public FindShippingOrderQueryHandler(IShippingOrderRepository shippingOrderRepository)
    {
        _shippingOrderRepository = shippingOrderRepository;
    }

    public async Task<ShippingOrder?> ExecuteAsync(Guid shippingOrderId)
    {
        return await _shippingOrderRepository.FindByIdAsync(shippingOrderId);
    }
}