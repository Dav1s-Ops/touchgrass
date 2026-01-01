using Touchgrass.Interfaces;

namespace Touchgrass.Services
{
    public class PomodoroConfig : IPomodoroConfig
    {
        public int Cycles { get; set; } = 4;
        public int WorkDurationSeconds { get; set; } = 25 * 60;
        public int BreakDurationSeconds { get; set; } = 5 * 60;
        public string? AlarmSound { get; set; }
    }
}
