using Spectre.Console;
using Touchgrass.Interfaces;
using Touchgrass.Utils;

namespace Touchgrass.Services
{
    public class SpectreConsoleUI : IConsoleUI
    {
        public void DisplayWelcome()
        {
            AnsiConsole.MarkupLine("[bold green]Touchgrass Pomodoro Timer[/]");
        }

        public void DisplayConfig(IPomodoroConfig config)
        {
            var panel = new Panel($"""
            [italic yellow]Cycles: [/][slowblink]{config.Cycles}[/]
            [italic yellow]Work:   [/][slowblink]{config.WorkDurationSeconds.ToMinSecString()}[/]
            [italic yellow]Break:  [/][slowblink]{config.BreakDurationSeconds.ToMinSecString()}[/]
            """);
            panel.Header = new PanelHeader("Params");
            panel.Border = BoxBorder.Ascii;
            AnsiConsole.Write(panel);
        }

        public void DisplayStatus(string phase, TimeSpan remaining)
        {
            // Used in spinner context
        }

        public void DisplayPhaseComplete(string phase, int cycle)
        {
            AnsiConsole.MarkupLine($"[bold]{phase} phase {cycle} complete![/]");
            AnsiConsole.MarkupLine("[italic green]Go touch some grass.[/]");
        }

        public void DisplayAllCyclesComplete()
        {
            AnsiConsole.MarkupLine("[bold green]All cycles complete![/]");
            AnsiConsole.MarkupLine("[italic purple]Hope you got some s*** done. See you next time![/]");
        }

        public void DisplayError(string message)
        {
            AnsiConsole.MarkupLine($"[red]{message}[/]");
        }

        public bool ConfirmContinue()
        {
            return AnsiConsole.Confirm("Continue to next phase?");
        }

        public string PromptRestartOrExit()
        {
            var prompt = new SelectionPrompt<string>()
                .Title("What next?")
                .AddChoices("Exit", "Restart");
            return AnsiConsole.Prompt(prompt);
        }

        public void RunStatusSpinner(Action spinnerAction, string startMessage)
        {
            AnsiConsole.Status()
                .Spinner(Spinner.Known.Star)
                .SpinnerStyle(Style.Parse("green bold"))
                .Start(startMessage, context =>
                {
                    spinnerAction();
                });
        }
    }
}
