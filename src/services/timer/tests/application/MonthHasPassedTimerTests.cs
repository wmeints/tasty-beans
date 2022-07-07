using FakeItEasy;
using TastyBeans.Shared.Application;
using TastyBeans.Timer.Application.Services;
using Xunit;

namespace TastyBeans.Timer.Application.Tests;

public class MonthHasPassedTimerTests
{
    [Fact]
    public void CanParseTimerExpression()
    {
        var eventPublisher = A.Fake<IEventPublisher>();
        var expressionText = "30 * * * * *";
        
        var timer = new MonthHasPassedTimer(expressionText, eventPublisher,null);
        
        Assert.NotNull(timer);
    }
}