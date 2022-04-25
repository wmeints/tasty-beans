using FluentValidation;
using RecommendCoffee.Ratings.Domain.Aggregates.CustomerAggregate;
using RecommendCoffee.Ratings.Domain.Aggregates.ProductAggregate;
using RecommendCoffee.Ratings.Domain.Aggregates.RatingAggregate.Commands;

namespace RecommendCoffee.Ratings.Domain.Aggregates.RatingAggregate.Validators;

public class RegisterRatingCommandValidator: AbstractValidator<RegisterRatingCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly ICustomerRepository _customerRepository;

    public RegisterRatingCommandValidator(IProductRepository productRepository, ICustomerRepository customerRepository)
    {
        _productRepository = productRepository;
        _customerRepository = customerRepository;

        RuleFor(x => x.Value).GreaterThanOrEqualTo(1).LessThanOrEqualTo(5);
        RuleFor(x => x.CustomerId).MustAsync(BeExistingCustomer);
        RuleFor(x => x.ProductId).MustAsync(BeExistingProduct);
    }

    private async Task<bool> BeExistingProduct(Guid productId, CancellationToken cancellationToken)
    {
        return await _productRepository.ExistsAsync(productId);
    }

    private async Task<bool> BeExistingCustomer(Guid customerId, CancellationToken cancellationToken)
    {
        return await _customerRepository.ExistsAsync(customerId);
    }
}