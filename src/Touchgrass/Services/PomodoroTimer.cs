using Touchgrass.Interfaces;

namespace Touchgrass.Services
{
    public class PomodoroTimer(IPomodoroConfig config) : IPomodoroTimer
    {
        private readonly IPomodoroConfig _config = config;

        public int Cycles => _config.Cycles;
        public int CurrentCycle { get; private set; } = 1;
        public int RemainingTime { get; private set; }
        public bool IsWorking { get; private set; }

        public void StartWork()
        {
            RemainingTime = _config.WorkDurationSeconds;
            IsWorking = true;
        }

        public void StartBreak()
        {
            RemainingTime = _config.BreakDurationSeconds;
            IsWorking = false;
        }

        public void Tick()
        {
            if (RemainingTime > 0)
                RemainingTime--;
        }

        public void SwitchPhase()
        {
            if (IsWorking)
            {
                StartBreak();
            }
            else
            {
                CurrentCycle++;
                StartWork();
            }
        }
    }
}
