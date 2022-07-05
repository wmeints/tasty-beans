using System;
using FluentAssertions;
using TastyBeans.Catalog.Domain.Aggregates.ProductAggregate;
using TastyBeans.Catalog.Domain.Aggregates.ProductAggregate.Events;
using Xunit;

namespace TastyBeans.Catalog.Domain.Tests.Aggregates.ProductAggregate;

public class ProductTests
{
    public class WhenRegistering
    {
        private Product _product;

        public WhenRegistering()
        {
            _product = new Product(Guid.NewGuid(), "Test", "Test");
        }

        [Fact]
        public void Then_TheProductReflectsTheInputData()
        {
            _product.Name.Should().Be("Test");
            _product.Description.Should().Be("Test");
        }

        [Fact]
        public void Then_NoValidationErrorsAreReturned()
        {
            _product.BusinessRuleViolations.Should().BeEmpty();
        }

        [Fact]
        public void Then_AProductRegisteredEventIsReturned()
        {
            _product.PendingDomainEvents.Should().ContainSingle(x => x is ProductRegisteredEvent);
        }
    }

    public class WhenUpdating
    {
        private Product _product;

        public WhenUpdating()
        {
            _product = new Product(Guid.NewGuid(),
                "Ultrablack 2000 (tm)",
                "The ultimate black coffee");

            _product.ClearPendingDomainEvents();

            _product.UpdateProductDetails("Mild and soft", "The milder variant");
        }

        [Fact]
        public void Then_UpdatesTheProduct()
        {
            _product.Name.Should().Be("Mild and soft");
            _product.Description.Should().Be("The milder variant");
        }

        [Fact]
        public void Then_NoErrorsAreReturned()
        {
            _product.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Then_AnProductUpdateEventIsReturned()
        {
            _product.PendingDomainEvents.Should().ContainSingle(x => x is ProductUpdatedEvent);
        }
    }

    public class WhenDiscontinuing
    {
        private Product _product;

        public WhenDiscontinuing()
        {
            _product = new Product(
                Guid.NewGuid(),
                "Weaksauce",
                "A very lame blend of coffee");

            _product.ClearPendingDomainEvents();

            _product.Discontinue();
        }

        [Fact]
        public void Then_TheProductIsDiscontinued()
        {
            _product.Discontinued.Should().BeTrue();
        }

        [Fact]
        public void Then_NoErrorsAreReturned()
        {
            _product.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Then_ADiscontinuedEventIsReturned()
        {
            _product.PendingDomainEvents.Should().ContainSingle(x => x is ProductDiscontinuedEvent);
        }
    }

    public class WhenTasteTesting
    {
        private Product _product;

        public WhenTasteTesting()
        {
            _product = new Product(Guid.NewGuid(), "Diesel",
                "Dark, rich and energizing, here’s fuel for your morning (or afternoon, or evening). " +
                "A bit of earthy smokiness gives it extra oomph.");

            _product.ClearPendingDomainEvents();

            _product.CompleteTasteTest("Comforting & rich", new[] {"Roastiness", "Ripe Fruit", "Milk Chocolate" }, 8);
        }

        [Fact]
        public void PublishesTasteTestedEvent()
        {
            _product.PendingDomainEvents.Should().ContainSingle(x => x is ProductTasteTestedEvent);
        }

        [Fact]
        public void RaisesNoErrors()
        {
            _product.IsValid.Should().BeTrue();
        }

        [Fact]
        public void UpdatesTasteInformation()
        {
            _product.Taste.Should().BeSameAs("Comforting & rich");
            _product.FlavorNotes.Should().BeEquivalentTo(new[] {"Roastiness", "Ripe Fruit", "Milk Chocolate" });
            _product.RoastLevel.Should().Be(8);
        }
    }
}