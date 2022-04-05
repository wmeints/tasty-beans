using FakeItEasy;
using FluentAssertions;
using RecommendCoffee.Ratings.Domain.Aggregates.CustomerAggregate;
using RecommendCoffee.Ratings.Domain.Aggregates.ProductAggregate;
using RecommendCoffee.Ratings.Domain.Aggregates.RatingAggregate;
using RecommendCoffee.Ratings.Domain.Aggregates.RatingAggregate.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Domain.Tests
{
    public class RatingTests
    {
        [Fact]
        public async Task CanRegisterRating()
        {
            var productRepository = A.Fake<IProductRepository>();   
            var customerRepository = A.Fake<ICustomerRepository>();

            A.CallTo(() => customerRepository.ExistsAsync(A<Guid>.Ignored)).Returns(true);
            A.CallTo(() => productRepository.ExistsAsync(A<Guid>.Ignored)).Returns(true);

            var cmd = new RegisterRatingCommand(Guid.NewGuid(), Guid.NewGuid(), 5);
            var response = await Rating.Register(cmd, productRepository, customerRepository);

            response.Rating.Should().NotBeNull();
            response.IsValid.Should().BeTrue();
        }
    }
}
