namespace RecommendCoffee.Transport.Domain.Aggregates.ShipmentAggregate;

public class TransportServiceLevelOptions
{
    public double ShipmentSortingLossChance { get; set; }
    public double ShipmentDeliveryDelayChance { get; set; }
    public double CustomerNotHomeChance { get; set; }
    public int MaxDeliveryAttempts { get; set; }
}