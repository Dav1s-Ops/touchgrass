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
                for (int i = 0; i < args.Length; i += 2)
                {
                    if (i + 1 >= args.Length)
                    {
                        AnsiConsole.MarkupLine("[red]Invalid arguments: Missing value for option.[/]");
                        return;
                    }

                    string option = args[i].ToLower();
                    if (!int.TryParse(args[i + 1], out int value) || value <= 0)
                    {
                        AnsiConsole.MarkupLine("[red]Invalid arguments: Value must be a positive integer.[/]");
                        return;
                    }

                    switch (option)
                    {
                        case "--work":
                            timer.WorkDuration = value * 60;
                            break;
                        case "--break":
                            timer.BreakDuration = value * 60;
                            break;
                        case "--cycles":
                            timer.Cycles = value;
                            break;
                        default:
                            AnsiConsole.MarkupLine($"[red]Unknown option: {option}[/]");
                            return;
                    }
                }
            }

            var panel = new Panel($"""
                [italic yellow]Cycles: [/][slowblink]{timer.Cycles}[/] 
                [italic yellow]Work:   [/][slowblink]{timer.WorkDuration.ToMinSecString()}[/] 
                [italic yellow]Break:  [/][slowblink]{timer.BreakDuration.ToMinSecString()}[/]
                """);
            panel.Header = new PanelHeader("Params");
            panel.Border = BoxBorder.Ascii;
            AnsiConsole.Write(panel);

            RunTimer(timer);
        }

        static void RunTimer(PomodoroTimer timer)
        {
            timer.StartWork();

            while (true)
            {
                if (timer.CurrentCycle == timer.Cycles)
                {
                    AnsiConsole.MarkupLine("\n[bold green]All cycles complete![/]");
                    AnsiConsole.MarkupLine("[italic darkgreen]Hope you got some s*** done. See you next time![/]");
                    if (AnsiConsole.Confirm("Exit?")) return;
                }
                var phase = timer.IsWorking ? "Work" : "Break";
                AnsiConsole.Status()
                    .Spinner(Spinner.Known.TimeTravel)
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

                AnsiConsole.MarkupLine($"[bold]{phase} phase {timer.CurrentCycle} complete![/]");
                AnsiConsole.MarkupLine("[italic darkgreen]Go touch some grass.[/]");
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