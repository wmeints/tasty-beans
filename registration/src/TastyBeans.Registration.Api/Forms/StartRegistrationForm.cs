using TastyBeans.Registration.Api.Models;

namespace TastyBeans.Registration.Api.Forms;

public record StartRegistrationForm(CustomerInfo CustomerDetails, PaymentMethodInfo PaymentMethod);