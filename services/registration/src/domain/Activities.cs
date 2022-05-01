using System.Diagnostics;

namespace TastyBeans.Registration.Domain;

public static class Activities
{
    private static readonly ActivitySource ActivitySource = new ActivitySource("TastyBeans.Registration.Domain");

    public static Activity? RegisterPaymentMethod(string currentState)
    {
        using var activity = ActivitySource.StartActivity(
            "RegisterPaymentMethod", ActivityKind.Internal);

        activity?.AddTag("statemachine.state", currentState);

        return activity;
    }
    
    public static Activity? RegisterCustomer(string currentState)
    {
        using var activity = ActivitySource.StartActivity(
            "RegisterCustomer", ActivityKind.Internal);

        activity?.AddTag("statemachine.state", currentState);

        return activity;
    }
    
    public static Activity? RegisterSubscription(string currentState)
    {
        using var activity = ActivitySource.StartActivity(
            "RegisterSubscription", ActivityKind.Internal);

        activity?.AddTag("statemachine.state", currentState);

        return activity;
    }

    public static Activity? SaveStateMachine(string currentState)
    {
        using var activity = ActivitySource.StartActivity(
            "SaveStateMachine", ActivityKind.Internal);

        activity?.AddTag("statemachine.state", currentState);

        return activity;
    }
    
    public static Activity? CompleteSubscriptionRegistration(string currentState)
    {
        using var activity = ActivitySource.StartActivity(
            "CompleteSubscriptionRegistration", ActivityKind.Internal);

        activity?.AddTag("statemachine.state", currentState);

        return activity;
    }
    
    public static Activity? CompletePaymentMethodRegistration(string currentState)
    {
        using var activity = ActivitySource.StartActivity(
            "CompletePaymentMethodRegistration", ActivityKind.Internal);

        activity?.AddTag("statemachine.state", currentState);

        return activity;
    }
    
    public static Activity? CompleteCustomerRegistration(string currentState)
    {
        using var activity = ActivitySource.StartActivity(
            "CompleteCustomerRegistration", ActivityKind.Internal);

        activity?.AddTag("statemachine.state", currentState);

        return activity;
    }
    
    public static Activity? StartRegistration(string currentState)
    {
        using var activity = ActivitySource.StartActivity(
            "StartRegistration", ActivityKind.Internal);

        activity?.AddTag("statemachine.state", currentState);

        return activity;
    }
}