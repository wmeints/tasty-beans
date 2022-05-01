using Akka.Actor;
using Microsoft.Extensions.Options;
using TastyBeans.Transport.Domain.Aggregates.ShipmentAggregate;
using TastyBeans.Transport.Domain.Aggregates.ShipmentAggregate.Commands;

namespace TastyBeans.Transport.Application.CommandHandlers;

public class CreateShipmentCommandHandler
{
    private readonly ActorSystem _actorSystem;
    private readonly IOptions<TransportServiceLevelOptions> _transportServiceLevelOptions;

    public CreateShipmentCommandHandler(ActorSystem actorSystem, IOptions<TransportServiceLevelOptions> transportServiceLevelOptions)
    {
        _actorSystem = actorSystem;
        _transportServiceLevelOptions = transportServiceLevelOptions;
    }

    public Task ExecuteAsync(CreateShipmentCommand cmd)
    {
        var shipmentActor = _actorSystem.ActorOf(Shipment.Props(_transportServiceLevelOptions.Value));
        shipmentActor.Tell(cmd, ActorRefs.NoSender);

        return Task.CompletedTask;
    }
}