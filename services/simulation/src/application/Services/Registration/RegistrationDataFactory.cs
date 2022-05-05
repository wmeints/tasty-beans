using Bogus;

namespace TastyBeans.Simulation.Application.Services.Registration;

public class RegistrationDataFactory
{
    private Faker _faker = new Faker("en");
    
    public RegistrationRequest Create()
    {
        var address = new Address(_faker.Address.StreetAddress(), _faker.Address.BuildingNumber(),
            _faker.Address.ZipCode(), _faker.Address.City(), _faker.Address.CountryCode());

        var lastName = _faker.Name.LastName();
        var firstName = _faker.Name.FirstName();

        var customerDetails = new CustomerDetails(firstName, lastName, _faker.Internet.Email(firstName, lastName),
            _faker.Phone.PhoneNumber("###-###-####"), address, address);

        var paymentMethod = new PaymentMethodDetails(CardType.Mastercard,
            _faker.Finance.CreditCardNumber(Bogus.DataSets.CardType.Mastercard).Replace("-", ""),
            _faker.Date.Future(1).ToString("MM/yy"),
            _faker.Finance.CreditCardCvv(),
            $"{firstName} {lastName}");

        var subscriptionDetails = new SubscriptionDetails(ShippingFrequency.Monthly, SubscriptionKind.OneYear);

        return new RegistrationRequest
        {
            CustomerDetails = customerDetails,
            Subscription = subscriptionDetails,
            PaymentMethod = paymentMethod
        };
    }
}