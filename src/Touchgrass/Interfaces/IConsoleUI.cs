using Spectre.Console;

namespace Touchgrass.Interfaces
{
    public interface IConsoleUI
    {
        void DisplayWelcome();
        void DisplayConfig(IPomodoroConfig config);
        void DisplayCycleTracking(int currentCycle, int completedWorks, int completedBreaks);
        void DisplayCycleComplete(int currentCycle, int completedWorks, int completedBreaks, string phase);
        void DisplayGrassMessage();
        void DisplayAllCyclesComplete();
        void DisplayCompletedStats(int completedWorks, int completedBreaks);
        void DisplayFinishedMessage();
        void DisplayError(string message);
        bool ConfirmContinue();
        string PromptRestartOrExit();
        void ClearOneLine();
        void ClearCurrentLine();
        void ClearLines(int num);
    }
}
