using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendCoffee.Catalog.Infrastructure.Persistence
{
    public class EventData<TKey>
    {
        private EventData()
        {

        }

        public EventData(Guid eventId, [DisallowNull] TKey aggregateId, string eventType, string data, DateTime dateCreated)
        {
            EventId = eventId;
            EventType = eventType;
            AggregateId = aggregateId;
            Data = data;
            Timestamp = dateCreated;
        }

        public Guid EventId { get; private set; }

        [NotNull]
        public TKey AggregateId { get; private set; }

        public string EventType { get; private set; }

        public string Data { get; private set; }

        public DateTime Timestamp { get; private set; }
    }
}
