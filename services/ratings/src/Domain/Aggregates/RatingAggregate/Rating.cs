using RecommendCoffee.Ratings.Domain.Aggregates.CustomerAggregate;
using RecommendCoffee.Ratings.Domain.Aggregates.ProductAggregate;
using RecommendCoffee.Ratings.Domain.Aggregates.RatingAggregate.Commands;
using RecommendCoffee.Ratings.Domain.Aggregates.RatingAggregate.Events;
using RecommendCoffee.Ratings.Domain.Aggregates.RatingAggregate.Validators;
using RecommendCoffee.Ratings.Domain.Common;

namespace RecommendCoffee.Ratings.Domain.Aggregates.RatingAggregate;

public class Rating
{
    public Rating(Guid id, Guid productId, Guid customerId, int value, DateTime dateCreated)
    {
        Id = id;
        ProductId = productId;
        CustomerId = customerId;
        Value = value;
        DateCreated = dateCreated;
    }

    public Guid Id { get; private set; }
    public Guid ProductId { get; private set; }
    public Guid CustomerId { get; private set; }
    public int Value { get; private set; }
    public DateTime DateCreated { get; private set; }

    public static async Task<RegisterRatingCommandResponse> Register(
        RegisterRatingCommand cmd, 
        IProductRepository productRepository, 
        ICustomerRepository customerRepository)
    {
        var validator = new RegisterRatingCommandValidator(productRepository, customerRepository);
        var validationResult = await validator.ValidateAsync(cmd);

        if (!validationResult.IsValid)
        {
            return new RegisterRatingCommandResponse(
                null,
                validationResult.GetValidationErrors(), 
                Enumerable.Empty<IDomainEvent>());
        }

        var rating = new Rating(
            Guid.NewGuid(), 
            cmd.ProductId, 
            cmd.CustomerId, 
            cmd.Value, 
            DateTime.UtcNow);

        var ratingRegisteredEvent = new RatingRegisteredEvent(
            rating.Id, rating.ProductId, rating.CustomerId, rating.Value);

        return new RegisterRatingCommandResponse(
            rating, Enumerable.Empty<ValidationError>(),
            new[] { ratingRegisteredEvent });
    }
}