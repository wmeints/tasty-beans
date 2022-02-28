namespace RecommendCoffee.Registration.Domain.Registrations;

public enum RegistrationState
{
    NotStarted,
    WaitingForCustomerRegistration,
    WaitingForPaymentMethodDetails,
    WaitingForPaymentMethodRegistration,
    WaitingForSubscriptionDetails,
    WaitingForSubscriptionRegistration,
    Completed
}