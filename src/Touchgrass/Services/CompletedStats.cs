namespace Touchgrass.Services
{
    public class CompletedStats
    {
        public int Works { get; private set; }
        public int Breaks { get; private set; }

        public void UpdateCompletedStats(PomodoroTimer timer)
        {
            if (timer.IsWorking)
            {
                Works++;
            }
            else
            {
                Breaks++;
            }
        }
    }
}
