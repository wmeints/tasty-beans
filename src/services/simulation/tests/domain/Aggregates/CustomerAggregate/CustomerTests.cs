using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.TestKit;
using Akka.TestKit.Xunit2;
using Moq;
using TastyBeans.Shipping.Domain.Aggregates.ShippingOrderAggregate;
using TastyBeans.Simulation.Domain.Aggregates.CustomerAggregate;
using TastyBeans.Simulation.Domain.Aggregates.CustomerAggregate.Commands;
using TastyBeans.Simulation.Domain.Services.Ratings;
using TastyBeans.Simulation.Domain.Services.ShippingInformation;
using TastyBeans.Simulation.Domain.Services.Subscriptions;
using Xunit;

namespace TastyBeans.Simulation.Domain.Tests.Aggregates.CustomerAggregate;

public class CustomerTests : TestKit
{
    private TestProbe? _testProbe;
    private IActorRef? _customer;
    private Mock<ISubscriptions> _subscriptions;
    private Mock<IRatings> _ratings;
    private Mock<IShippingInformation> _shippingInformation;
    private Guid _customerId;
    private readonly Guid _shippingOrderId;

    public CustomerTests()
    {
        _customerId = Guid.NewGuid();
        _shippingOrderId = Guid.NewGuid();
        _shippingInformation = new Mock<IShippingInformation>();
        _ratings = new Mock<IRatings>();
        _subscriptions = new Mock<ISubscriptions>();

        _customer = Sys.ActorOf(Customer.Props(
            _customerId, new CustomerProfile(), _shippingInformation.Object,
            _ratings.Object, _subscriptions.Object));

        _testProbe = CreateTestProbe();

        _shippingInformation
            .Setup(x => x.GetShippingOrderAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new ShippingOrder()
            {
                Id = _shippingOrderId,
                CustomerId = _customerId,
                OrderDate = DateTime.Now.AddHours(-1),
                OrderItems = new List<OrderItem>()
                {
                    new OrderItem() { Amount = 1, ProductId = Guid.NewGuid() }
                }
            });
    }

    [Fact]
    public void CanHandleShipmentDelivered()
    {
        _customer.Tell(new ShipmentDelivered(_shippingOrderId), _testProbe);
        _testProbe.ExpectMsg<OK>(TimeSpan.FromMilliseconds(100));

        _shippingInformation.Verify(x => x.GetShippingOrderAsync(It.IsAny<Guid>()));
        _ratings.Verify(x => x.RateProductAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<int>()));
    }

    [Fact]
    public void CanHandleShipmentLost()
    {
        _customer.Tell(new ShipmentLost(_shippingOrderId), _testProbe);
        _testProbe.ExpectMsg<OK>(TimeSpan.FromMilliseconds(100));
    }

    [Theory]
    [MemberData(nameof(GetTestMessages))]
    public void RepliesToMessagesWithOk(object message)
    {
        _customer.Tell(message, _testProbe);
        _testProbe.ExpectMsg<OK>(TimeSpan.FromMilliseconds(100));
    }

    public static IEnumerable<object[]> GetTestMessages()
    {
        var orderId = Guid.NewGuid();

        yield return new object[] { new ShipmentDelivered(orderId) };
        yield return new object[] { new ShipmentLost(orderId) };
        yield return new object[] { new DeliveryDelayed(orderId) };
        yield return new object[] { new DeliveryAttemptFailed(orderId) };
        yield return new object[] { new DriverOutForDelivery(orderId) };
        yield return new object[] { new ShipmentSent(orderId) };
    }
}