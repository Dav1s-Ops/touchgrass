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
        public string Phase { get; private set; } = "Work";

        public void StartWork()
        {
            RemainingTime = _config.WorkDurationSeconds;
            IsWorking = true;
            Phase = "Work";
        }

        public void StartBreak()
        {
            RemainingTime = _config.BreakDurationSeconds;
            IsWorking = false;
            Phase = "Break";
        }

        public void Tick()
        {
            if (RemainingTime > 0)
                RemainingTime--;
        }

        public void StartPhase()
        {
            if (IsWorking)
            {
                StartWork();
            }
            else
            {
                StartBreak();
            }
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
