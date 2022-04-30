using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Microsoft.Extensions.Options;
using RecommendCoffee.Transport.Domain.Aggregates.ShipmentAggregate;
using RecommendCoffee.Transport.Domain.Aggregates.ShipmentAggregate.Commands;

namespace RecommendCoffee.Transport.Application.CommandHandlers;

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