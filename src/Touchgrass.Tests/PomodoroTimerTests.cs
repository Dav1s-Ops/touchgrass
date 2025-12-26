using Touchgrass.Interfaces;
using Touchgrass.Services;

namespace Touchgrass.Tests;

public class PomodoroTimerTests
{
    [Fact]
    public void Timer_InitWithDefaultDurations()
    {
        var config = new PomodoroConfig();
        var timer = new PomodoroTimer(config);
        Assert.Equal(25 * 60, config.WorkDurationSeconds);
        Assert.Equal(5 * 60, config.BreakDurationSeconds);
        Assert.Equal(1, timer.CurrentCycle);
    }

    [Fact]
    public void Timer_CountsDownWorkPeriod()
    {
        var config = new PomodoroConfig();
        var timer = new PomodoroTimer(config);
        timer.StartWork();
        timer.Tick();
        Assert.Equal(25 * 60 - 1, timer.RemainingTime);
        Assert.True(timer.IsWorking);
    }

    [Fact]
    public void Timer_AdvanceCycleAndSwitchesToBreak()
    {
        var config = new PomodoroConfig();
        var timer = new PomodoroTimer(config);
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
        var config = new PomodoroConfig();
        var timer = new PomodoroTimer(config);
        timer.StartBreak();
        timer.SwitchPhase();
        Assert.True(timer.IsWorking);
        Assert.Equal(25 * 60, timer.RemainingTime);
    }

    [Fact]
    public void StartPhase_ResetsToFullWorkDuration_WhenIsWorkingTrue()
    {
        IPomodoroConfig config = new PomodoroConfig();
        IPomodoroTimer timer = new PomodoroTimer(config);

        timer.StartWork();
        Assert.Equal(25 * 60, timer.RemainingTime);
        Assert.True(timer.IsWorking);
        Assert.Equal("Work", timer.Phase);

        // Simulate partial countdown
        timer.Tick();
        Assert.Equal(25 * 60 - 1, timer.RemainingTime);

        // Act: Reset to current phase
        timer.StartPhase();

        // Assert: Resets to full work duration, preserves phase
        Assert.Equal(25 * 60, timer.RemainingTime);
        Assert.True(timer.IsWorking);
        Assert.Equal("Work", timer.Phase);
    }

    [Fact]
    public void StartPhase_ResetsToFullBreakDuration_WhenIsWorkingFalse()
    {
        IPomodoroConfig config = new PomodoroConfig();
        IPomodoroTimer timer = new PomodoroTimer(config);

        timer.StartWork();
        timer.SwitchPhase(); // Switch to break phase
        Assert.Equal(5 * 60, timer.RemainingTime);
        Assert.False(timer.IsWorking);
        Assert.Equal("Break", timer.Phase);

        // Simulate partial countdown
        timer.Tick();
        Assert.Equal(5 * 60 - 1, timer.RemainingTime);

        // Act: Reset to current phase
        timer.StartPhase();

        // Assert: Resets to full break duration, preserves phase
        Assert.Equal(5 * 60, timer.RemainingTime);
        Assert.False(timer.IsWorking);
        Assert.Equal("Break", timer.Phase);
    }
}
