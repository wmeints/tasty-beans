using System;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using TastyBeans.Ratings.Domain.Aggregates.CustomerAggregate;
using TastyBeans.Ratings.Domain.Aggregates.ProductAggregate;
using TastyBeans.Ratings.Domain.Aggregates.RatingAggregate;
using TastyBeans.Ratings.Domain.Aggregates.RatingAggregate.Commands;
using Xunit;

namespace TastyBeans.Ratings.Domain.Tests.Aggregates.RatingAggregate
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
