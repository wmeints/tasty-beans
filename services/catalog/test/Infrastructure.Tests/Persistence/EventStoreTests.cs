using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using RecommendCoffee.Catalog.Domain.Aggregates.ProductAggregate;
using RecommendCoffee.Catalog.Domain.Aggregates.ProductAggregate.Events;
using RecommendCoffee.Catalog.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace RecommendCoffee.Catalog.Infrastructure.Tests.Persistence
{
    public class EventStoreTests
    {
        [Fact]
        public async Task PersistAsync_ValidEvent_StoresEvent()
        {
            var dbContext = CreateApplicationDbContext();

            var eventStore = new EventStore<Product, Guid>(dbContext);
            var aggregateId = Guid.NewGuid();

            var domainEvent = new ProductRegistered(
                aggregateId,
                "test",
                "test",
                new[] { new ProductVariant(500, 7.71m) }
            );

            await eventStore.PersistAsync(aggregateId, new[] { domainEvent });

            var eventData = dbContext.ProductEvents.Single();

            eventData.Should().NotBeNull();
            eventData.Data.Should().BeEquivalentTo(JsonSerializer.Serialize(domainEvent));
            eventData.AggregateId.Should().Be(aggregateId);
            eventData.EventType.Should().BeEquivalentTo(domainEvent.GetType().AssemblyQualifiedName);
            eventData.Timestamp.Should().Be(domainEvent.OccurredOn);
        }

        [Fact]
        public async Task LoadAsync_WithValidInput_ReturnsEvents()
        {
            var dbContext = CreateApplicationDbContext();

            var eventStore = new EventStore<Product, Guid>(dbContext);
            var aggregateId = Guid.NewGuid();

            var domainEvent = new ProductRegistered(
                aggregateId,
                "test",
                "test",
                new[] { new ProductVariant(500, 7.71m) }
            );

            var eventData = new EventData<Guid>(
                Guid.NewGuid(),
                aggregateId,
                typeof(ProductRegistered).AssemblyQualifiedName!,
                JsonSerializer.Serialize(domainEvent),
                DateTime.UtcNow);

            dbContext.Set<EventData<Guid>>("ProductEvents").Add(eventData);
            dbContext.SaveChanges();

            var events = await eventStore.LoadAsync(aggregateId);

            events.Should().HaveCount(1);
        }

        private ApplicationDbContext CreateApplicationDbContext()
        {
            var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("TestDatabase")
                .Options;

            var dbContext = new ApplicationDbContext(dbContextOptions);

            return dbContext;
        }
    }
}
