using System;
using System.Collections.Generic;
using Akka.Actor;
using Akka.TestKit;
using Akka.TestKit.Xunit2;
using Moq;
using TastyBeans.Shipping.Domain.Aggregates.ShippingOrderAggregate;
using TastyBeans.Simulation.Application.Services.Registration;
using TastyBeans.Simulation.Application.Services.Simulation;
using TastyBeans.Simulation.Domain.Aggregates.CustomerAggregate;
using TastyBeans.Simulation.Domain.Aggregates.CustomerAggregate.Commands;
using TastyBeans.Simulation.Domain.Services.Ratings;
using TastyBeans.Simulation.Domain.Services.ShippingInformation;
using TastyBeans.Simulation.Domain.Services.Subscriptions;
using Xunit;

namespace TastyBeans.Simulation.Application.Tests;

public class SimulatorTests : TestKit
{
    private Mock<IRegistration> _registration;
    private Mock<IRatings> _ratings;
    private Mock<ISubscriptions> _subscriptions;
    private Mock<IShippingInformation> _shippingInformation;
    private IActorRef? _simulatorRef;
    private TestProbe _testProbe;

    public SimulatorTests()
    {
        _registration = new Mock<IRegistration>();
        _ratings = new Mock<IRatings>();
        _subscriptions = new Mock<ISubscriptions>();
        _shippingInformation = new Mock<IShippingInformation>();

        _shippingInformation.Setup(x => x.GetShippingOrderAsync(It.IsAny<Guid>())).ReturnsAsync(new ShippingOrder()
        {
            CustomerId = Guid.Empty,
            Id = Guid.NewGuid(),
            OrderItems = new List<OrderItem>
            {
                new OrderItem { Amount = 1, ProductId = Guid.NewGuid() }
            }
        });

        _simulatorRef = Sys.ActorOf(Simulator.Props(
            _registration.Object, _shippingInformation.Object, _ratings.Object, _subscriptions.Object));

        _testProbe = CreateTestProbe();
    }

    [Fact]
    public void CanHandleShipmentLostEvent()
    {
        _simulatorRef.Tell(new StartSimulation(1, new()
        {
            new WeightedCustomerProfile { Weight = 1.0, Profile = new CustomerProfile() }
        }), _testProbe);

        var customerId = Guid.NewGuid();
        var shippingOrderId = Guid.NewGuid();

        _simulatorRef.Tell(new CustomerRegistered(customerId), ActorRefs.NoSender);
        _simulatorRef.Tell(new ShippingOrderCreated(customerId, shippingOrderId), ActorRefs.Nobody);
        _simulatorRef.Tell(new ShipmentLost(shippingOrderId), _testProbe);

        Within(TimeSpan.FromSeconds(1), () => { _testProbe.ExpectMsg<OK>(); });
    }

    [Fact]
    public void CanHandleShipmentDeliveredEvent()
    {
        _simulatorRef.Tell(new StartSimulation(1, new()
        {
            new WeightedCustomerProfile { Weight = 1.0, Profile = new CustomerProfile() }
        }), _testProbe);

        var customerId = Guid.NewGuid();
        var shippingOrderId = Guid.NewGuid();

        _simulatorRef.Tell(new CustomerRegistered(customerId), ActorRefs.NoSender);
        _simulatorRef.Tell(new ShippingOrderCreated(customerId, shippingOrderId));
        _simulatorRef.Tell(new ShipmentDelivered(shippingOrderId), _testProbe);

        _testProbe.ExpectMsg<OK>(TimeSpan.FromSeconds(3));
    }
}