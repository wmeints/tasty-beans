using System;
using Bogus;

using TastyBeans.Simulation.Application.IntegrationEvents;

namespace TastyBeans.Simulation.Api.Tests.Support;

public class EventObjectFactory
{
    public static CustomerRegisteredEvent Create()
    {
        return new CustomerRegisteredEvent(Guid.NewGuid());
    }

    public static ShipmentDeliveredEvent CreateShipmentDeliveredEvent()
    {
        return new ShipmentDeliveredEvent(Guid.NewGuid());
    }

    public static ShipmentLostEvent CreateShipmentLostEvent()
    {
        return new ShipmentLostEvent(Guid.NewGuid());
    }
}