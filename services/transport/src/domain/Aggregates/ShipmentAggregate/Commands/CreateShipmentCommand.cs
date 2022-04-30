using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendCoffee.Transport.Domain.Aggregates.ShipmentAggregate.Commands
{
    public record CreateShipmentCommand(Guid ShippingOrderId);
}
