using RecommendCoffee.Catalog.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendCoffee.Catalog.Application.Persistence
{
    public class AggregateNotFoundException: Exception
    {
        private AggregateNotFoundException(string message): base(message)
        {

        }

        public static AggregateNotFoundException Create<TAggregate, TKey>(TKey aggregateId) where TAggregate: AggregateRoot<TKey>
        {
            var message = $"Could not find {typeof(TAggregate).Name} with key {aggregateId}";
            return new AggregateNotFoundException(message);
        }
    }
}
