using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Touchgrass
{
    public class PomodoroTimer
    {
        public int WorkDuration { get; set; } = 25 * 60;
        public int BreakDuration { get; set; } = 5 * 60;
        public int CurrentCycle { get; set; } = 0;
    }
}
