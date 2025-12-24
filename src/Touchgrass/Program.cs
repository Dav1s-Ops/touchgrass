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
                        case "--testing":
                            timer.Cycles = 1;
                            timer.BreakDuration = value;
                            timer.WorkDuration = value;
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
                    """)
            {
                Header = new PanelHeader("Params"),
                Border = BoxBorder.Ascii,
            };
            AnsiConsole.Write(panel);

            RunTimer(timer);
        }

        private static void WriteControl(string code)
        {
            AnsiConsole.Write(new Text(code));
        }

        static void RunTimer(PomodoroTimer timer)
        {
            int completedWorks = 0;
            int completedBreaks = 0;
            timer.StartWork();

            while (true)
            {
                if (timer.CurrentCycle > timer.Cycles)
                {
                    AnsiConsole.MarkupLine("[bold green]All cycles complete![/]");
                    AnsiConsole.MarkupLine($"[italic yellow]Works: [/]{completedWorks} | [italic yellow]Breaks: [/]{completedBreaks}");
                    AnsiConsole.MarkupLine("[italic purple]Hope you got some s*** done. See you next time![/]");

                    var prompt = new SelectionPrompt<string>()
                        .Title("What next?")
                        .AddChoices("Exit", "Restart");

                    var choice = AnsiConsole.Prompt(prompt);

                    if (choice == "Exit") return;
                    if (choice == "Restart")
                    {
                        WriteControl("\u001b[1A\u001b[2K"); // Clear confirm line
                        WriteControl("\u001b[1A\u001b[2K"); // Clear grass line
                        WriteControl("\u001b[1A\u001b[2K"); // Clear complete line, now cursor at tracking position
                        timer.CurrentCycle = 1;
                        completedWorks = 0;
                        completedBreaks = 0;
                        timer.StartWork();
                        continue;
                    }
                }

                var phase = timer.IsWorking ? "Work" : "Break";

                AnsiConsole.MarkupLine($"[italic yellow]Cycle: [/]{timer.CurrentCycle} | [italic yellow]Works: [/]{completedWorks} | [italic yellow]Breaks: [/]{completedBreaks}");

                AnsiConsole.Status()
                    .Spinner(Spinner.Known.Star)
                    .SpinnerStyle(Style.Parse("green bold"))
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

                if (timer.IsWorking)
                {
                    completedWorks++;
                }
                else
                {
                    completedBreaks++;
                }

                WriteControl("\u001b[1A\u001b[2K");
                AnsiConsole.MarkupLine($"[italic yellow]Cycle: [/]{timer.CurrentCycle} | [italic yellow]Works: [/]{completedWorks} | [italic yellow]Breaks: [/]{completedBreaks} | [italic purple]{phase} phase complete![/]");

                WriteControl("\u001b[2K");
                AnsiConsole.MarkupLine("[italic green]Go touch some grass.[/]");

                timer.SwitchPhase();

                bool continueNext = AnsiConsole.Confirm("Continue to next phase?");

                if (!continueNext)
                {
                    break;
                }

                // Clean up after confirm
                WriteControl("\u001b[1A\u001b[2K"); // Clear confirm line
                WriteControl("\u001b[1A\u001b[2K"); // Clear grass line
                WriteControl("\u001b[1A\u001b[2K"); // Clear complete line, now cursor at tracking position
                                                    // Ready for next tracking print in loop
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