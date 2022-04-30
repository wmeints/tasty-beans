using FakeItEasy;
using RecommendCoffee.Shared.Application;
using RecommendCoffee.Timer.Application.Services;
using Xunit;

namespace RecommendCoffee.Timer.Application.Tests;

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