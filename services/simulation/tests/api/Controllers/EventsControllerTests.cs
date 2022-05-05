using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using TastyBeans.Simulation.Api.Tests.Support;
using Moq;
using TastyBeans.Simulation.Application.Services;
using Xunit;

namespace TastyBeans.Simulation.Api.Tests.Controllers;

public class EventsControllerTests
{
    private WebApplicationFactory<Program> _application;
    private HttpClient _client;
    
    [NotNull]
    private Mock<ISimulation>? _simulation;

    public EventsControllerTests()
    {
        _application = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    _simulation = services.ReplaceWithMock<ISimulation>();
                });
            });

        _client = _application.CreateClient();

        _simulation.Setup(x => x.IsRunningAsync()).ReturnsAsync(true);
    }

    [Fact]
    public async Task CanHandleCustomerRegisteredEvent()
    {
        var evt = EventObjectFactory.Create();

        var response = await _client.PostCloudEventAsync(
            "/events/CustomerRegistered",
            "customermanagement.customer.registered.v1",
            evt);

        response.EnsureSuccessStatusCode();

        _simulation.Verify(x => x.CustomerRegisteredAsync(It.Is<Guid>(
            x => x == evt.CustomerId)));
    }

    [Fact]
    public async Task CanHandleShipmentDeliveredEvent()
    {
        var evt = EventObjectFactory.CreateShipmentDeliveredEvent();

        var response = await _client.PostCloudEventAsync(
            "/events/ShipmentDelivered",
            "transport.shipment.delivered.v1",
            evt);

        response.EnsureSuccessStatusCode();

        _simulation.Verify(x => x.ShipmentDeliveredAsync(
            It.Is<Guid>(x => x == evt.ShippingOrderId)));
    }
    
    [Fact]
    public async Task CanHandleShipmentLostEvent()
    {
        var evt = EventObjectFactory.CreateShipmentLostEvent();

        var response = await _client.PostCloudEventAsync(
            "/events/ShipmentLost",
            "transport.shipment.lost.v1",
            evt);

        response.EnsureSuccessStatusCode();

        _simulation.Verify(x => x.ShipmentLostAsync(
            It.Is<Guid>(x => x == evt.ShippingOrderId)));
    }
}