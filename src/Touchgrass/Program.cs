using Spectre.Console;
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

        private static void RunTimer(
            PomodoroTimer timer, 
            SpectreConsoleUI ui, 
            IPomodoroConfig config
        )
        {
            var stats = new CompletedStats();
            timer.StartWork();

            while (true)
            {
                if (timer.CurrentCycle > config.Cycles)
                {
                    ui.DisplayAllCyclesComplete();
                    ui.DisplayCompletedStats(stats);
                    ui.DisplayFinishedMessage();

                    var choice = ui.PromptRestartOrExit();

                    if (choice == "Exit") return;
                    if (choice == "Restart")
                    {
                        ui.ClearLines(3);
                        timer = new PomodoroTimer(config);
                        stats = new CompletedStats();
                        timer.StartWork();
                        continue;
                    }
                }

                ui.DisplayCycleTracking(timer.CurrentCycle, stats);

                AnsiConsole.Status()
                    .Spinner(Spinner.Known.TimeTravel)
                    .SpinnerStyle(Style.Parse("green bold"))
                    .Start($"{timer.Phase} session starting...", context =>
                    {
                        timer.StartPhase();

                        while (timer.RemainingTime > 0)
                        {
                            Thread.Sleep(1000);
                            timer.Tick();
                            context.Status = $"[yellow]{timer.Phase}: {TimeSpan.FromSeconds(timer.RemainingTime):mm\\:ss}[/]";
                            context.Refresh();
                        }
                    });

                stats.UpdateCompletedStats(timer);

                ui.ClearOneLine();
                ui.DisplayCycleComplete(timer.CurrentCycle, stats, timer.Phase);

                ui.ClearCurrentLine();
                ui.DisplayGrassMessage();

                timer.SwitchPhase();

                bool continueNext = ui.ConfirmContinue();

                if (!continueNext) break;

                ui.ClearLines(3);
            }
        }
    }
}