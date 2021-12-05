using RecommendCoffee.Customers.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendCoffee.Customers.Domain.Aggregates.CustomerAggregate.Events
{
    [Topic("customers.fct.customer-registered.v1")]
    public record CustomerRegistered(Guid Id, string FirstName, string LastName, Address InvoiceAddress, Address ShippingAddress) : Event;
}
