namespace Touchgrass.Interfaces
{
    public interface IConsoleUI
    {
        void DisplayWelcome();
        void DisplayConfig(IPomodoroConfig config);
        void DisplayStatus(string phase, TimeSpan remaining);
        void DisplayPhaseComplete(string phase, int cycle);
        void DisplayAllCyclesComplete();
        void DisplayError(string message);
        bool ConfirmContinue();
        string PromptRestartOrExit();
        void RunStatusSpinner(Action spinnerAction, string startMessage);
    }
}
