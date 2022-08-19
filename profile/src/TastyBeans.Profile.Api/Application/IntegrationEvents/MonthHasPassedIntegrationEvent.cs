using Jasper.Attributes;

namespace TastyBeans.Profile.Api.Application.IntegrationEvents;

[MessageIdentity("timer.month-has-passed.v1")]
public record MonthHasPassed(DateTime Date);