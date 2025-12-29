using Touchgrass.Services;

namespace Touchgrass.Interfaces
{
    public interface ICompletedStats
    {
        int Works { get; }
        int Breaks { get; }
        void UpdateCompletedStats(PomodoroTimer timer);
    }
}
