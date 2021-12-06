using RecommendCoffee.Customers.Domain.Aggregates.CustomerAggregate.Events;
using RecommendCoffee.Customers.Domain.Common;

namespace RecommendCoffee.Customers.Domain.Projections.CustomerInfoProjection;

public class CustomerInformationProjector : EventProjector
{
    private readonly ICustomerInformationRepository _customerInformationRepository;

    public CustomerInformationProjector(ICustomerInformationRepository customerInformationRepository)
    {
        _customerInformationRepository = customerInformationRepository;
    }

    protected override async Task ApplyEvent(Event @event)
    {
        switch (@event)
        {
            case CustomerRegistered evt:
                await OnCustomerRegistered(evt);
                break;
        }
    }

    private async Task OnCustomerRegistered(CustomerRegistered evt)
    {
        var customerInformation = new CustomerInformation
        {
            Id = evt.CustomerId,
            FirstName = evt.FirstName,
            LastName = evt.LastName,
            InvoiceAddress = new AddressInformation
            {
                City = evt.InvoiceAddress.City,
                HouseNumber = evt.InvoiceAddress.HouseNumber,
                StreetName = evt.InvoiceAddress.StreetName,
                ZipCode = evt.InvoiceAddress.ZipCode
            },
            ShippingAddress = new AddressInformation
            {
                City = evt.InvoiceAddress.City,
                HouseNumber = evt.InvoiceAddress.HouseNumber,
                StreetName = evt.InvoiceAddress.StreetName,
                ZipCode = evt.InvoiceAddress.ZipCode
            }
        };

        await _customerInformationRepository.Insert(customerInformation);
    }
}