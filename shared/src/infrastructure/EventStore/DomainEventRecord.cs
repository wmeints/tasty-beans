namespace TastyBeans.Shared.Infrastructure.EventStore;

public record DomainEventRecord(long Id, Guid AggregateId, long SequenceNumber, string PayloadType, string PayloadData);