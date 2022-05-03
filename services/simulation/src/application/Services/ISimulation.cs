namespace TastyBeans.Simulation.Application.Services;

public interface ISimulation
{
    Task StartSimulationAsync(int customerCount);
    Task<bool> IsRunningAsync();
    Task DeliveryAttemptFailedAsync(Guid shippingOrderId);
    Task DeliveryDelayedAsync(Guid shippingOrderId);
    Task DriverOutForDeliveryAsync(Guid shippingOrderId);
    Task ShipmentDeliveredAsync(Guid evtShippingOrderId);
    Task ShipmentLostAsync(Guid shippingOrderId);
    Task ShipmentReturnedAsync(Guid shippingOrderId);
    Task ShipmentSentAsync(Guid shippingOrderId);
    Task ShipmentSortedAsync(Guid shippingOrderId);
    Task ShippingOrderCreated(Guid customerId, Guid shippingOrderId);
    Task CustomerRegisteredAsync(Guid customerId);
}