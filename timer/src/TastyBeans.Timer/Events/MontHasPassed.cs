using Jasper.Attributes;

namespace TastyBeans.Timer.Events;

[MessageIdentity("timer.month-has-passed.v1")]
public record MontHasPassed(DateTime Date);