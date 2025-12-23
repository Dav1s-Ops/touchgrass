namespace Touchgrass.Tests;

public class PomodoroTimerTests
{
    [Fact]
    public void Timer_InitWithDefaultDurations()
    {
        var timer = new PomodoroTimer();
        Assert.Equal(25 * 60, timer.WorkDuration);
        Assert.Equal(5 * 60, timer.BreakDuration);
        Assert.Equal(0, timer.CurrentCycle);
    }
}
