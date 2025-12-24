using Touchgrass.Interfaces;
using Touchgrass.Services;

namespace Touchgrass
{
    class Program
    {
        static void Main(string[] args)
        {
            var parser = new CommandLineParser();
            var ui = new SpectreConsoleUI();

            ui.DisplayWelcome();

            IPomodoroConfig config;
            try
            {
                config = parser.Parse(args);
            }
            catch (ArgumentException ex)
            {
                ui.DisplayError(ex.Message);
                return;
            }

            ui.DisplayConfig(config);

            var timer = new PomodoroTimer(config);

            RunTimer(timer, ui, config);
        }

        private static void RunTimer(IPomodoroTimer timer, IConsoleUI ui, IPomodoroConfig config)
        {
            timer.StartWork();

            while (true)
            {
                if (timer.CurrentCycle > config.Cycles)
                {
                    ui.DisplayAllCyclesComplete();
                    var choice = ui.PromptRestartOrExit();

                    if (choice == "Exit") return;
                    if (choice == "Restart")
                    {
                        timer = new PomodoroTimer(config); // Reset
                        timer.StartWork();
                        continue;
                    }
                }

                var phase = timer.IsWorking ? "Work" : "Break";
                ui.RunStatusSpinner(() =>
                {
                    while (timer.RemainingTime > 0)
                    {
                        Thread.Sleep(1000);
                        timer.Tick();
                    }
                }, $"{phase} session starting...");

                // Update status inside a separate loop for clarity, but since spinner wraps it, we handle ticks inside
                ui.DisplayPhaseComplete(phase, timer.CurrentCycle);
                timer.SwitchPhase();
                if (!ui.ConfirmContinue()) break;
            }
        }
    }
}