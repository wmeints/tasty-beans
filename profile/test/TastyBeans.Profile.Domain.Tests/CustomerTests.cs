using TastyBeans.Profile.Domain.Aggregates.CustomerAggregate;
using TastyBeans.Profile.Domain.Aggregates.CustomerAggregate.Events;

namespace TastyBeans.Profile.Domain.Tests;

public class CustomerTests
{
    [Fact]
    public void CanRegisterNewCustomer()
    {
        var shippingAddress = new Address("Test", "1", "1234 AA", "Test");
        var invoiceAddres = new Address("Test", "1", "1234 AA", "Test");
        
        var customer = Customer.Register(Guid.NewGuid(),
            "Mike",
            "DrinksATonOfCoffee",
            shippingAddress,
            invoiceAddres,
            "test@domain.org",
            DateTime.UtcNow);

        Assert.NotNull(customer);
        Assert.Equal("Mike", customer.FirstName);
        Assert.Equal("DrinksATonOfCoffee", customer.LastName);
        Assert.Equal("test@domain.org", customer.EmailAddress);
        Assert.Equal(shippingAddress, customer.ShippingAddress);
        Assert.Equal(invoiceAddres, customer.InvoiceAddress);

        Assert.Single(customer.PendingDomainEvents.Where(x => x is CustomerRegistered));
        Assert.Single(customer.PendingDomainEvents.Where(x => x is CustomerSubscribed));
    }

    [Fact]
    public void CanCancelSubscription()
    {
        var customerId = Guid.NewGuid();
        var customer = CreateRegisteredCustomer(customerId);
        
        customer.Unsubscribe(DateTime.UtcNow);
        
        Assert.NotNull(customer.Subscription);
        Assert.NotNull(customer.Subscription!.EndDate);
        Assert.Single(customer.PendingDomainEvents.Where(x => x is CustomerUnsubscribed));
    }

    [Fact]
    public void CanEndSubscription()
    {
        var customerId = Guid.NewGuid();
        var customer = CreateRegisteredCustomerWithPendingCancellation(customerId);
        
        customer.EndSubscription();
        
        Assert.Null(customer.Subscription);
    }

    [Fact]
    public void CanResumeSubscription()
    {
        var customerId = Guid.NewGuid();
        var customer = CreateRegisteredCustomerWithPendingCancellation(customerId);
        
        customer.Subscribe(DateTime.UtcNow);
        
        Assert.NotNull(customer.Subscription);
        Assert.Null(customer.Subscription!.EndDate);
    }

    private Customer CreateRegisteredCustomer(Guid customerId)
    {
        var customer = Customer.Register(customerId,
            "Mike",
            "DrinksATonOfCoffee",
            new Address("Test", "1", "1234 AA", "Test"),
            new Address("Test", "1", "1234 AA", "Test"),
            "test@domain.org",
            DateTime.UtcNow);
        
        customer.ClearPendingDomainEvents();

        return customer;
    }
    
    private Customer CreateRegisteredCustomerWithPendingCancellation(Guid customerId)
    {
        var customer = CreateRegisteredCustomer(customerId);
        customer.Unsubscribe(DateTime.UtcNow);
        
        customer.ClearPendingDomainEvents();

        return customer;
    }
}