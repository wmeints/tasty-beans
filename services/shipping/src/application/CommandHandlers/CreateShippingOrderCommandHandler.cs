using RecommendCoffee.Shipping.Application.Services;
using RecommendCoffee.Shipping.Domain.Aggregates.ShippingOrderAggregate;
using RecommendCoffee.Shipping.Domain.Aggregates.ShippingOrderAggregate.Commands;

namespace RecommendCoffee.Shipping.Application.CommandHandlers;

public class CreateShippingOrderCommandHandler
{
    private readonly IShippingOrderRepository _shippingOrderRepository;
    private readonly IEventPublisher _eventPublisher;
    private readonly ITransportCompany _transportCompany;

    public CreateShippingOrderCommandHandler(IShippingOrderRepository shippingOrderRepository,
        IEventPublisher eventPublisher, ITransportCompany transportCompany)
    {
        _shippingOrderRepository = shippingOrderRepository;
        _eventPublisher = eventPublisher;
        _transportCompany = transportCompany;
    }

    public async Task<CreateShippingOrderCommandResponse> ExecuteAsync(CreateShippingOrderCommand cmd)
    {
        using var activity = Activities.ExecuteCommand("CreateShippingOrder");
        var response = ShippingOrder.Create(cmd);

        if (response.IsValid)
        {
            await _shippingOrderRepository.InsertAsync(response.Order);
            await _eventPublisher.PublishEventsAsync(response.Events);
            await _transportCompany.ShipAsync(response.Order.Id);
        }

        return response;
    }
}