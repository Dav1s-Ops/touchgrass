using Touchgrass.Services;

namespace Touchgrass.Interfaces
{
    public interface IConsoleUI
    {
        void DisplayWelcome();
        void DisplayConfig(IPomodoroConfig config);
        void DisplayCycleTracking(int currentCycle, CompletedStats stats);
        void DisplayCycleComplete(int currentCycle, CompletedStats stats, string phase);
        void DisplayGrassMessage();
        void DisplayAllCyclesComplete();
        void DisplayCompletedStats(CompletedStats stats);
        void DisplayFinishedMessage();
        void DisplayError(string message);
        bool ConfirmContinue();
        string PromptRestartOrExit();
        void ClearOneLine();
        void ClearCurrentLine();
        void ClearLines(int num);
    }
}
