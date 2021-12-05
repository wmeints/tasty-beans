using FluentAssertions;
using RecommendCoffee.Customers.Domain.Aggregates.CustomerAggregate;
using RecommendCoffee.Customers.Domain.Aggregates.CustomerAggregate.Commands;
using RecommendCoffee.Customers.Domain.Aggregates.CustomerAggregate.Events;
using RecommendCoffee.Customers.Domain.Common;
using System;
using Xunit;

namespace Domain.Tests.Aggregates.CustomerAggregate;

public class CustomerTests
{
    [Fact]
    public void LoadCustomer_ValidEvents_ReturnsCustomer()
    {
        var customerId = Guid.NewGuid();
        var shippingAddress = new Address("SomeStreet", "12", "0000 AA", "SomeCity");
        var invoiceAddress = new Address("SomeStreet", "12", "0000 AA", "SomeCity");

        var events = new[]
        {
            new CustomerRegistered(customerId, "Joe", "Plumber", invoiceAddress, shippingAddress)
        };

        var customer = Customer.Load(customerId, events);

        customer.Should().NotBeNull();
        customer.ShippingAddress.Should().BeEquivalentTo(shippingAddress);
        customer.InvoiceAddress.Should().BeEquivalentTo(invoiceAddress);
        customer.FirstName.Should().Be("Joe");
        customer.LastName.Should().Be("Plumber");
    }

    [Fact]
    public void RegisterCustomer_ValidInput_ReturnsCustomer()
    {
        var shippingAddress = new Address("SomeStreet", "12", "0000 AA", "SomeCity");
        var invoiceAddress = new Address("SomeStreet", "12", "0000 AA", "SomeCity");

        var cmd = new RegisterCustomerCommand("Joe","Plumber", invoiceAddress, shippingAddress);

        var customer = Customer.Register(cmd);

        customer.Should().NotBeNull();
        customer.ShippingAddress.Should().BeEquivalentTo(shippingAddress);
        customer.InvoiceAddress.Should().BeEquivalentTo(invoiceAddress);
        customer.FirstName.Should().Be("Joe");
        customer.LastName.Should().Be("Plumber");
    }

    [Fact]
    public void RegisterCustomer_ValidationError_RaisesException()
    {
        var shippingAddress = new Address("SomeStreet", "12", "0000 AA", "SomeCity");
        var invoiceAddress = new Address("SomeStreet", "12", "0000 AA", "SomeCity");

        var cmd = new RegisterCustomerCommand("", "Plumber", invoiceAddress, shippingAddress);

        Assert.Throws<BusinessRulesViolationException>(() =>
        {
            var customer = Customer.Register(cmd);
        });
    }
}
