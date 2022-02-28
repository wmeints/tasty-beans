namespace RecommendCoffee.Registration.Domain.Registrations;

public enum RegistrationTrigger
{
    RegisterCustomerDetails,
    CompleteCustomerRegistration,
    SubmitPaymentMethodDetails,
    CompletePaymentMethodRegistration,
    CompleteSubscriptionRegistration
}