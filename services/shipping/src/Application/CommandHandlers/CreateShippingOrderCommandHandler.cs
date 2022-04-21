using RecommendCoffee.Shipping.Application.Common;
using RecommendCoffee.Shipping.Domain.Aggregates.ShippingOrderAggregate;
using RecommendCoffee.Shipping.Domain.Aggregates.ShippingOrderAggregate.Commands;

namespace RecommendCoffee.Shipping.Application.CommandHandlers;

public class CreateShippingOrderCommandHandler
{
    private readonly IShippingOrderRepository _shippingOrderRepository;
    private readonly IEventPublisher _eventPublisher;

    public CreateShippingOrderCommandHandler(IShippingOrderRepository shippingOrderRepository, IEventPublisher eventPublisher)
    {
        _shippingOrderRepository = shippingOrderRepository;
        _eventPublisher = eventPublisher;
    }

    public async Task<CreateShippingOrderCommandResponse> ExecuteAsync(CreateShippingOrderCommand cmd)
    {
        using var activity = Activities.ExecuteCommand("CreateShippingOrder");
        var response = ShippingOrder.Create(cmd);

        if (response.IsValid)
        {
            await _shippingOrderRepository.InsertAsync(response.Order);
            await _eventPublisher.PublishEventsAsync(response.Events);
        }

        return response;
    }
}