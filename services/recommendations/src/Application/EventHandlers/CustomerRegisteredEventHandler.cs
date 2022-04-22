using RecommendCoffee.Recommendations.Application.Common;
using RecommendCoffee.Recommendations.Application.IntegrationEvents;
using RecommendCoffee.Recommendations.Domain.Aggregates.CustomerAggregate;
using RecommendCoffee.Recommendations.Domain.Aggregates.CustomerAggregate.Commands;

namespace RecommendCoffee.Recommendations.Application.EventHandlers;

public class CustomerRegisteredEventHandler
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerRegisteredEventHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task HandleAsync(CustomerRegisteredEvent evt)
    {
        using var activity = Activities.HandleEvent("customermanagement.customer.registered.v1");
        var response = Customer.Register(new RegisterCustomerCommand(
                evt.CustomerId,
                evt.FirstName,
                evt.LastName,
                evt.EmailAddress,
                new Domain.Aggregates.CustomerAggregate.Address(
                    evt.ShippingAddress.Street,
                    evt.ShippingAddress.HouseNumber,
                    evt.ShippingAddress.PostalCode,
                    evt.ShippingAddress.City,
                    evt.ShippingAddress.CountryCode
                )
            )
        );

        if (!response.IsValid)
        {
            throw new EventValidationFailedException(response.Errors);
        }

        await _customerRepository.InsertAsync(response.Customer);
    }
}