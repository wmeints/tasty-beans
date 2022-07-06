using System.ComponentModel.DataAnnotations;
using System.IO.Compression;
using Microsoft.Extensions.Logging;
using TastyBeans.CustomerManagement.Domain.Aggregates.CustomerAggregate;
using TastyBeans.CustomerManagement.Domain.Aggregates.CustomerAggregate.Commands;
using TastyBeans.Shared.Application;

namespace TastyBeans.CustomerManagement.Application.Services;

public class CustomerGenerationService : ICustomerGenerationService
{
    private readonly ICustomerSampleRepository _customerSampleRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly ILogger<CustomerGenerationService> _logger;
    private readonly IEventPublisher _eventPublisher;

    public CustomerGenerationService(ICustomerSampleRepository customerSampleRepository,
        ICustomerRepository customerRepository, ILogger<CustomerGenerationService> logger,
        IEventPublisher eventPublisher)
    {
        _customerSampleRepository = customerSampleRepository;
        _customerRepository = customerRepository;
        _logger = logger;
        _eventPublisher = eventPublisher;
    }

    public async Task GenerateAsync(string filename)
    {
        var customerSamples = await _customerSampleRepository.GetCustomerSamples(filename);

        foreach (var customerSample in customerSamples)
        {
            var customer = await _customerRepository.FindById(Guid.Parse(customerSample.Id));

            if (customer != null)
            {
                continue;
            }

            var streetName = Faker.Address.StreetName();
            var houseNumber = Faker.RandomNumber.Next(1, 150).ToString();
            var postalCode = Faker.Address.ZipCode();
            var city = Faker.Address.City();
            var countryCode = "US";

            var invoiceAddress = new Address(
                streetName,
                houseNumber,
                postalCode,
                city,
                countryCode);

            var shippingAddress = new Address(
                streetName,
                houseNumber,
                postalCode,
                city,
                countryCode);

            var emailAddress = Faker.Internet.Email($"{customerSample.FirstName} {customerSample.LastName}");
            var telephoneNumber = Faker.Phone.Number();

            var response = Customer.Register(new RegisterCustomerCommand(
                Guid.Parse(customerSample.Id),
                customerSample.FirstName,
                customerSample.LastName,
                invoiceAddress,
                shippingAddress,
                emailAddress,
                telephoneNumber));

            if (!response.IsValid)
            {
                _logger.LogWarning("Can't generate customer: {Errors}", response.Errors);
                continue;
            }

            await _customerRepository.InsertAsync(response.Customer);
            await _eventPublisher.PublishEventsAsync(response.Events);
        }
    }
}