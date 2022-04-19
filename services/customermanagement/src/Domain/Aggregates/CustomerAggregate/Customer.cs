using RecommendCoffee.CustomerManagement.Domain.Aggregates.CustomerAggregate.Commands;
using RecommendCoffee.CustomerManagement.Domain.Aggregates.CustomerAggregate.Events;
using RecommendCoffee.CustomerManagement.Domain.Aggregates.CustomerAggregate.Validators;
using RecommendCoffee.CustomerManagement.Domain.Common;

namespace RecommendCoffee.CustomerManagement.Domain.Aggregates.CustomerAggregate;

public class Customer
{
    private Customer()
    {
    }

    public Customer(Guid id, string firstName, string lastName, Address invoiceAddress, Address shippingAddress,
        string emailAddress, string telephoneNumber)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        InvoiceAddress = invoiceAddress;
        ShippingAddress = shippingAddress;
        EmailAddress = emailAddress;
        TelephoneNumber = telephoneNumber;
    }

    public Guid Id { get; private set; } = Guid.Empty;
    public string FirstName { get; private set; } = "";
    public string LastName { get; private set; } = "";
    public Address InvoiceAddress { get; private set; } = new Address("", "", "", "", "");
    public Address ShippingAddress { get; private set; } = new Address("", "", "", "", "");
    public string EmailAddress { get; private set; } = "";
    public string TelephoneNumber { get; private set; } = "";

    public static RegisterCustomerCommandReply Register(RegisterCustomerCommand command)
    {
        var validator = new RegisterCustomerCommandValidator();
        var validationResult = validator.Validate(command);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(x => new ValidationError(x.PropertyName, x.ErrorMessage))
                .ToList();
            return new RegisterCustomerCommandReply(null, errors, Enumerable.Empty<IDomainEvent>());
        }

        var instance = new Customer(
            command.CustomerId,
            command.FirstName,
            command.LastName,
            command.InvoiceAddress,
            command.ShippingAddress,
            command.EmailAddress,
            command.TelephoneNumber);

        var evt = new CustomerRegisteredEvent(
            instance.Id,
            instance.FirstName,
            instance.LastName,
            instance.InvoiceAddress,
            instance.ShippingAddress,
            instance.EmailAddress,
            instance.TelephoneNumber);

        return new RegisterCustomerCommandReply(instance, Enumerable.Empty<ValidationError>(),
            new IDomainEvent[] { evt });
    }
}