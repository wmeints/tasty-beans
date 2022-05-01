using Akka.Actor;
using Akka.Event;
using TastyBeans.Transport.Domain.Aggregates.ShipmentAggregate.Commands;
using TastyBeans.Transport.Domain.Aggregates.ShipmentAggregate.Events;

namespace TastyBeans.Transport.Domain.Aggregates.ShipmentAggregate;

public class Shipment : FSM<ShipmentStatus, IShipmentData>
{
    private readonly Random _random = new Random();

    public Shipment(TransportServiceLevelOptions serviceLevelOptions)
    {
        StartWith(ShipmentStatus.WaitingForGoods, new NoShipmentDetails());

        When(ShipmentStatus.WaitingForGoods, state =>
        {
            if (state.FsmEvent is CreateShipmentCommand cmd)
            {
                Context.System.EventStream.Publish(new ShipmentSentEvent(cmd.ShippingOrderId));
                Self.Tell(new StartSortingCommand());

                return GoTo(ShipmentStatus.Pending).Using(new ShipmentData(cmd.ShippingOrderId, 1));
            }

            return null;
        });

        When(ShipmentStatus.Pending, state =>
        {
            var logger = Context.GetLogger();
            var shipmentData = (ShipmentData)state.StateData;

            if (state.FsmEvent is StartSortingCommand cmd)
            {
                logger.Info("Shipment {ShippingOrderId} is being sorted", shipmentData.ShippingOrderId);

                Self.Tell(new CompleteSortingCommand());

                return GoTo(ShipmentStatus.Sorting).Using(state.StateData);
            }

            return null;
        });

        When(ShipmentStatus.Sorting, state =>
        {
            var logger = Context.GetLogger();
            var shipmentData = (ShipmentData)state.StateData;

            if (state.FsmEvent is CompleteSortingCommand cmd)
            {
                if (_random.NextDouble() < serviceLevelOptions.ShipmentSortingLossChance)
                {
                    logger.Warning("Shipment {ShippingOrderId} is lost.", shipmentData.ShippingOrderId);

                    Context.System.EventStream.Publish(new ShipmentLostEvent(shipmentData.ShippingOrderId));

                    return Stop(Normal.Instance);
                }
                else
                {
                    logger.Info("Shipment {ShippingOrderId} is sorted.", shipmentData.ShippingOrderId);

                    Context.System.EventStream.Publish(new ShipmentSortedEvent(shipmentData.ShippingOrderId));
                    Self.Tell(new StartDeliveryCommand());

                    return GoTo(ShipmentStatus.Sorted).Using(shipmentData);
                }
            }

            return null;
        });

        When(ShipmentStatus.Sorted, state =>
        {
            var logger = Context.GetLogger();
            var shipmentData = (ShipmentData)state.StateData;

            if (state.FsmEvent is StartDeliveryCommand cmd)
            {
                logger.Info("Driver is out for delivery of shipment {ShippingOrderId}", shipmentData.ShippingOrderId);

                Context.System.EventStream.Publish(new DriverOutForDeliveryEvent(shipmentData.ShippingOrderId));
                Self.Tell(new CompleteDeliveryCommand());

                return GoTo(ShipmentStatus.OutForDelivery).Using(shipmentData);
            }

            return null;
        });

        When(ShipmentStatus.OutForDelivery, state =>
        {
            var logger = Context.GetLogger();
            var shipmentData = (ShipmentData)state.StateData;

            if (state.FsmEvent is CompleteDeliveryCommand cmd)
            {
                if (_random.NextDouble() < serviceLevelOptions.ShipmentDeliveryDelayChance)
                {
                    logger.Warning("Delivery of shipment {ShippingOrderId} is delayed", shipmentData.ShippingOrderId);

                    Context.System.EventStream.Publish(new DeliveryDelayedEvent(shipmentData.ShippingOrderId));
                    Self.Tell(new CompleteDeliveryCommand());

                    return GoTo(ShipmentStatus.OutForDelivery).Using(shipmentData);
                }

                if (_random.NextDouble() < serviceLevelOptions.CustomerNotHomeChance)
                {
                    if (shipmentData.Attempts < serviceLevelOptions.MaxDeliveryAttempts)
                    {
                        logger.Warning("Deliver attempt {Attempt} failed for shipment {ShippingOrderId}. Trying again.");

                        Context.System.EventStream.Publish(new DeliveryAttemptFailedEvent());
                        Self.Tell(new CompleteDeliveryCommand());

                        return GoTo(ShipmentStatus.OutForDelivery).Using(shipmentData with { Attempts = shipmentData.Attempts + 1 });
                    }
                    else
                    {
                        logger.Error("Failed to deliver shipment {ShippingOrderId}", shipmentData.ShippingOrderId);

                        Context.System.EventStream.Publish(new ShipmentReturnedEvent(shipmentData.ShippingOrderId));

                        return Stop(Normal.Instance);
                    }
                }

                logger.Info("Shipment {ShippingOrderId} was succesfully delivered after {Attempts} attempts.",
                    shipmentData.ShippingOrderId, shipmentData.Attempts);

                Context.System.EventStream.Publish(new ShipmentDeliveredEvent(shipmentData.ShippingOrderId));

                return Stop(Normal.Instance);
            }

            return null;
        });

        Initialize();
    }

    public static Props Props(TransportServiceLevelOptions options) => new Props(
        type: typeof(Shipment),
        supervisorStrategy: new OneForOneStrategy(ex => Directive.Restart),
        args: new object[] { options });
}