using RecommendCoffee.Recommendations.Domain.Aggregates.CustomerAggregate.Validators;
using RecommendCoffee.Recommendations.Domain.Aggregates.CustomerAggregate.Commands;
using RecommendCoffee.Recommendations.Domain.Common;

namespace RecommendCoffee.Recommendations.Domain.Aggregates.CustomerAggregate;

public class Customer
{
    private Customer()
    {
        
    }

    private Customer(Guid id, string firstName, string lastName, string emailAddress, Address shippingAddress)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        EmailAddress = emailAddress;
        ShippingAddress = shippingAddress;
    }

    public Guid Id { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string EmailAddress { get; private set; }
    public Address ShippingAddress { get; private set; }
    
    public static RegisterCustomerCommandResponse Register(RegisterCustomerCommand cmd)
    {
        using var activity = Activities.RegisterProduct(cmd.Id);

        var validator = new RegisterCustomerCommandValidator();
        var validationResult = validator.Validate(cmd);

        if (!validationResult.IsValid)
        {
            return new RegisterCustomerCommandResponse(
                null, validationResult.GetValidationErrors());
        }

        var customer = new Customer(cmd.Id, cmd.FirstName, cmd.LastName,cmd.EmailAddress, cmd.ShippingAddress);

        return new RegisterCustomerCommandResponse(customer, Enumerable.Empty<ValidationError>());
    }
}