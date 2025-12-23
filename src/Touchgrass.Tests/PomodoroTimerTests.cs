namespace Touchgrass.Tests;

public class PomodoroTimerTests
{
    [Fact]
    public void Timer_InitWithDefaultDurations()
    {
        var timer = new PomodoroTimer();
        Assert.Equal(25 * 60, timer.WorkDuration);
        Assert.Equal(5 * 60, timer.BreakDuration);
        Assert.Equal(1, timer.CurrentCycle);
    }

    [Fact]
    public void Timer_CountsDownWorkPeriod()
    {
        var timer = new PomodoroTimer();
        timer.StartWork();
        timer.Tick();
        Assert.Equal(25 * 60 - 1, timer.RemainingTime);
        Assert.True(timer.IsWorking);
    }

    [Fact]
    public void Timer_AdvanceCycleAndSwitchesToBreak()
    {
        var timer = new PomodoroTimer();
        timer.StartWork();
        while (timer.RemainingTime > 0)
            timer.Tick();
        timer.SwitchPhase();
        Assert.Equal(1, timer.CurrentCycle);
        Assert.False(timer.IsWorking);
        Assert.Equal(5 * 60, timer.RemainingTime);
    }

    [Fact]
    public void Timer_SwitchesFromBreakToWork()
    {
        var timer = new PomodoroTimer();
        timer.StartBreak();
        timer.SwitchPhase();
        Assert.True(timer.IsWorking);
        Assert.Equal(25 * 60, timer.RemainingTime);
    }

}
