using RecommendCoffee.Customers.Domain.Aggregates.CustomerAggregate.Commands;
using RecommendCoffee.Customers.Domain.Aggregates.CustomerAggregate.Events;
using RecommendCoffee.Customers.Domain.Aggregates.CustomerAggregate.Validators;
using RecommendCoffee.Customers.Domain.Common;
using System.Diagnostics.CodeAnalysis;

namespace RecommendCoffee.Customers.Domain.Aggregates.CustomerAggregate;

public class Customer : AggregateRoot<Guid>
{
    private Customer(Guid id, IEnumerable<Event> events) : base(id)
    {
        ReplayDomainEvents(events);
    }

    private Customer(Guid id) : base(id)
    {

    }

    [NotNull]
    public string? FirstName { get; private set; }

    [NotNull]
    public string? LastName { get; private set; }

    [NotNull]
    public Address? InvoiceAddress { get; private set; }

    [NotNull]
    public Address? ShippingAddress { get; private set; }

    public static Customer Load(Guid id, IEnumerable<Event> events)
    {
        return new Customer(id, events);
    }

    public static Customer Register(RegisterCustomerCommand cmd)
    {
        var validator = new RegisterCustomerCommandValidator();
        var validationResult = validator.Validate(cmd);

        if(!validationResult.IsValid)
        {
            var violations = validationResult.Errors.Select(x=> new BusinessRuleViolation(x.PropertyName, x.ErrorMessage));
            throw new BusinessRulesViolationException(violations);
        }

        var customerId = Guid.NewGuid();
        var customer = new Customer(customerId);

        var customerRegistered = new CustomerRegistered(customerId, cmd.FirstName, cmd.LastName, cmd.InvoiceAddress, cmd.ShippingAddress);

        customer.ApplyEvent(customerRegistered);
        customer.PublishEvent(customerRegistered);

        return customer;
    }

    protected override Action GetEventHandler(Event @event) => @event switch
    {
        CustomerRegistered customerRegistered => () => OnCustomerRegistered(customerRegistered),
        _ => throw new InvalidOperationException($"Requested event {@event.GetType()} is not supported by this aggregate.")
    };

    private void OnCustomerRegistered(CustomerRegistered evt)
    {
        FirstName = evt.FirstName;
        LastName = evt.LastName;
        InvoiceAddress = evt.InvoiceAddress;
        ShippingAddress = evt.ShippingAddress;
    }
}
