namespace RecommendCoffee.Registration.Domain.Registrations;

public enum RegistrationState
{
    NotStarted,
    WaitingForCustomerRegistration,
    WaitingForPaymentMethodRegistration,
    WaitingForSubscriptionRegistration,
    Completed
}