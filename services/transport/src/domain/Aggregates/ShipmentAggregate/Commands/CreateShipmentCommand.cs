namespace TastyBeans.Transport.Domain.Aggregates.ShipmentAggregate.Commands
{
    public record CreateShipmentCommand(Guid ShippingOrderId);
}
