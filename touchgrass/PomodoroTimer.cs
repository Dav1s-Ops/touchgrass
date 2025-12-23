using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Touchgrass
{
    public class PomodoroTimer
    {
        public int Cycles { get; set; } = 4;
        public int WorkDuration { get; set; } = 25 * 60;
        public int BreakDuration { get; set; } = 5 * 60;
        public int CurrentCycle { get; set; } = 1;
        public int RemainingTime { get; private set; }
        public bool IsWorking { get; private set; }

        public void StartWork()
        {
            RemainingTime = WorkDuration;
            IsWorking = true;
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

        public void StartBreak()
        {
            RemainingTime = BreakDuration;
            IsWorking = false;
        }
    }
}
