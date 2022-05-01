using TastyBeans.Ratings.Domain.Aggregates.CustomerAggregate;
using TastyBeans.Ratings.Domain.Aggregates.ProductAggregate;
using TastyBeans.Ratings.Domain.Aggregates.RatingAggregate;
using TastyBeans.Ratings.Domain.Aggregates.RatingAggregate.Commands;
using TastyBeans.Shared.Application;

namespace TastyBeans.Ratings.Application.CommandHandlers;

public class RegisterRatingCommandHandler
{
    private readonly IRatingRepository _ratingRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IProductRepository _productRepository;
    private readonly IEventPublisher _eventPublisher;

    public RegisterRatingCommandHandler(
        IRatingRepository ratingRepository, 
        ICustomerRepository customerRepository, 
        IProductRepository productRepository,
        IEventPublisher eventPublisher)
    {
        _ratingRepository = ratingRepository;
        _customerRepository = customerRepository;
        _productRepository = productRepository;
        _eventPublisher = eventPublisher;
    }

    public async Task<RegisterRatingCommandResponse> ExecuteAsync(RegisterRatingCommand command)
    {
        using var activity = Activities.ExecuteCommand("RegisterRating");
        var response = await Rating.Register(command, _productRepository, _customerRepository);
        
        if(response.IsValid)
        {
            await _ratingRepository.InsertAsync(response.Rating);
            await _eventPublisher.PublishEventsAsync(response.Events);
            
            Metrics.RatingsRegistered.Add(1);
        }

        return response;
    }
}
