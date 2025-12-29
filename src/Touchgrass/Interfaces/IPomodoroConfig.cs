namespace Touchgrass.Interfaces
{
    public interface IPomodoroConfig
    {
        int Cycles { get; }
        int WorkDurationSeconds { get; }
        int BreakDurationSeconds { get; }
    }
}
