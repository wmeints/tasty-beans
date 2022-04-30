namespace RecommendCoffee.Transport.Domain.Aggregates.ShipmentAggregate;

public record ShipmentData(Guid ShippingOrderId, int Attempts) : IShipmentData;