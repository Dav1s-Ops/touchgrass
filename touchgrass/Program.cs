using Spectre.Console;

namespace Touchgrass
{
    class Program
    {
        static void Main(string[] args)
        {
            AnsiConsole.MarkupLine("[bold green]Touchgrass Pomodoro Timer[/]");
            var timer = new PomodoroTimer();

            // parse args for custom durations e.g. tgrass --work 30 --break 10
            if (args.Length > 0)
            {
                //TODO: Parse em!
            }

            AnsiConsole.MarkupLineInterpolated($"""
                [italic yellow]Cycles: [/][slowblink]{timer.Cycles}[/] 
                [italic yellow]Work:   [/][slowblink]{timer.WorkDuration.ToMinSecString()}[/] 
                [italic yellow]Break:  [/][slowblink]{timer.BreakDuration.ToMinSecString()}[/]
                """);

            RunTimer(timer);
        }

        static void RunTimer(PomodoroTimer timer)
        {
            timer.StartWork();

            while (true)
            {
                var phase = timer.IsWorking ? "Work" : "Break";
                AnsiConsole.Status()
                    .Spinner(Spinner.Known.FingerDance)
                    .Start($"{phase} session starting...", context =>
                    {
                        DetermineTimerPhase(timer);

                        while (timer.RemainingTime > 0)
                        {
                            Thread.Sleep(1000);
                            timer.Tick();
                            context.Status($"[yellow]{phase}: {TimeSpan.FromSeconds(timer.RemainingTime):mm\\:ss}[/]");
                            context.Refresh();
                        }
                    });

                AnsiConsole.Markup($"[bold]{phase} complete![/]\n");
                timer.SwitchPhase();
                if (!AnsiConsole.Confirm("Continue to next phase?")) break;
            }
        }

        public static void DetermineTimerPhase(PomodoroTimer timer)
        {
            if (timer.IsWorking)
            {
                timer.StartWork();
            }
            else
            {
                timer.StartBreak();
            }
        }
    }
}