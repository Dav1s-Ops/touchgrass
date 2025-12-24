namespace Touchgrass.Interfaces
{
    public interface IPomodoroTimer
    {
        int CurrentCycle { get; }
        int RemainingTime { get; }
        bool IsWorking { get; }
        void StartWork();
        void StartBreak();
        void Tick();
        void SwitchPhase();
    }
}
