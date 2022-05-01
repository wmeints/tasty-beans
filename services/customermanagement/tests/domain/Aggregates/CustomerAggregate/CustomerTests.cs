using System;
using FluentAssertions;
using TastyBeans.CustomerManagement.Domain.Aggregates.CustomerAggregate;
using TastyBeans.CustomerManagement.Domain.Aggregates.CustomerAggregate.Commands;
using Xunit;

namespace TastyBeans.CustomerManagement.Domain.Tests.Aggregates.CustomerAggregate;

public class CustomerTests
{
    [Fact]
    public void CanRegisterCustomer()
    {
        var address = new Address("Sample street", "19b", "1234 AC", "Building city", "NL");
        var command = new RegisterCustomerCommand(
            Guid.NewGuid(),
            "Bob",
            "The Builder", 
            address,
            address,
            "test@domain.org",
            "+310000000"); 
            
        var response = Customer.Register(command);

        response.Should().NotBeNull();
        response.Customer.Should().NotBeNull();

        response.Customer.InvoiceAddress.Should().Be(address);
        response.Customer.ShippingAddress.Should().Be(address);
        response.Customer.FirstName.Should().Be(command.FirstName);
        response.Customer.LastName.Should().Be(command.LastName);
        response.Customer.TelephoneNumber.Should().Be(command.TelephoneNumber);
        response.Customer.EmailAddress.Should().Be(command.EmailAddress);
    }
}
