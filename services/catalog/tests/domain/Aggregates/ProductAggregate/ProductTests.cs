using System;
using FluentAssertions;
using TastyBeans.Catalog.Domain.Aggregates.ProductAggregate;
using TastyBeans.Catalog.Domain.Aggregates.ProductAggregate.Commands;
using TastyBeans.Catalog.Domain.Aggregates.ProductAggregate.Events;
using Xunit;

namespace TastyBeans.Catalog.Domain.Tests.Aggregates.ProductAggregate;

public class ProductTests
{
    public class WhenRegistering
    {
        private Register _command;
        private RegisterProductCommandResponse _response;

        public WhenRegistering()
        {
            _command = new Register(
                "Ultra black 3000(tm)",
                "So dark, it will burn a hole in your coffee cup",
                new[] { new ProductVariant(500, 12.95m) });

            _response = Product.Register(_command);
        }

        [Fact]
        public void Then_TheProductIsReturned()
        {
            _response.Product.Should().NotBeNull();
        }

        [Fact]
        public void Then_TheProductReflectsTheInputData()
        {
            _response.Product.Name.Should().Be(_command.Name);
            _response.Product.Description.Should().Be(_command.Description);
        }

        [Fact]
        public void Then_NoValidationErrorsAreReturned()
        {
            _response.Errors.Should().BeEmpty();
        }

        [Fact]
        public void Then_AProductRegisteredEventIsReturned()
        {
            _response.Events.Should().ContainSingle(x => x is Registered);
        }
    }

    public class WhenUpdating
    {
        private Product _product;
        private UpdateProductCommand _command;
        private UpdateProductCommandResponse _response;

        public WhenUpdating()
        {
            _product = new Product(Guid.NewGuid(),
                "Ultrablack 2000 (tm)",
                "The ultimate black coffee",
                new[]
                {
                    new ProductVariant(250, 5.67m)
                });

            _command = new UpdateProductCommand(
                _product.Id,
                "Ultrablack 3000 (tm)",
                "So black it burns a hole in your coffee cup.", new[]
                {
                    new ProductVariant(500, 12.95m)
                });

            _response = _product.Update(_command);
        }

        [Fact]
        public void Then_UpdatesTheProduct()
        {
            _product.Name.Should().Be(_command.Name);
            _product.Description.Should().Be(_command.Description);
            _product.Variants.Should().Contain(_command.Variants);
        }

        [Fact]
        public void Then_NoErrorsAreReturned()
        {
            _response.Errors.Should().BeEmpty();
        }

        [Fact]
        public void Then_AnProductUpdateEventIsReturned()
        {
            _response.Events.Should().ContainSingle(x => x is ProductUpdatedEvent);
        }
    }

    public class WhenDiscontinuing
    {
        private Product _product;
        private Discontinue _command;
        private DiscontinueProductCommandResponse _response;

        public WhenDiscontinuing()
        {
            _product = new Product(
                Guid.NewGuid(),
                "Weaksauce",
                "A very lame blend of coffee",
                new[]
                {
                    new ProductVariant(1000, 12.95m)
                });

            _command = new Discontinue(_product.Id);
            _response = _product.Discontinue(_command);
        }

        [Fact]
        public void Then_TheProductIsDiscontinued()
        {
            _product.Discontinued.Should().BeTrue();
        }

        [Fact]
        public void Then_NoErrorsAreReturned()
        {
            _response.Errors.Should().BeEmpty();
        }

        [Fact]
        public void Then_ADiscontinuedEventIsReturned()
        {
            _response.Events.Should().ContainSingle(x => x is Discontinued);
        }
    }

    public class WhenTasteTesting
    {
        private Product _product;
        private readonly TasteTestProductCommandResponse _response;
        private readonly CompleteTasteTest _productCommand;

        public WhenTasteTesting()
        {
            var result = Product.Register(new Register(
                Name: "Diesel",
                Description:
                "Dark, rich and energizing, here’s fuel for your morning (or afternoon, or evening). A bit of earthy smokiness gives it extra oomph.",
                Variants: new[] { new ProductVariant(310, 17.40m), new ProductVariant(1000, 41.20m) }));

            _product = result.Product;

            _productCommand = new CompleteTasteTest(_product.Id, 8, "Comforting & Rich",
                new[] { "Roastiness", "Ripe Fruit", "Milk Chocolate" });
            
            _response = _product.TasteTest(_productCommand);
        }

        [Fact]
        public void PublishesTasteTestedEvent()
        {
            _response.Events.Should().ContainSingle(x => x is TasteTestCompleted);
        }

        [Fact]
        public void RaisesNoErrors()
        {
            _response.IsValid.Should().BeTrue();
        }

        [Fact]
        public void UpdatesTasteInformation()
        {
            _product.Taste.Should().BeSameAs(_productCommand.Taste);
            _product.FlavorNotes.Should().BeEquivalentTo(_productCommand.FlavorNotes);
            _product.RoastLevel.Should().Be(_productCommand.RoastLevel);
        }
    }
}