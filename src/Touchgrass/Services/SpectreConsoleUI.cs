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
            """)
            {
                Header = new PanelHeader("Params"),
                Border = BoxBorder.Ascii,
            };
            AnsiConsole.Write(panel);
        }

        public void DisplayCycleTracking(int currentCycle, CompletedStats stats)
        {
            AnsiConsole.MarkupLine($"[italic yellow]Cycle: [/]{currentCycle} | [italic yellow]Works: [/]{stats.Works} | [italic yellow]Breaks: [/]{stats.Breaks}");
        }

        public void DisplayCycleComplete(int currentCycle, CompletedStats stats, string phase)
        {
            AnsiConsole.MarkupLine($"[italic yellow]Cycle: [/]{currentCycle} | [italic yellow]Works: [/]{stats.Works} | [italic yellow]Breaks: [/]{stats.Breaks} | [italic purple]{phase} phase complete![/]");
        }

        public void DisplayGrassMessage()
        {
            AnsiConsole.MarkupLine("[italic green]Go touch some grass.[/]");
        }

        public void DisplayAllCyclesComplete()
        {
            AnsiConsole.MarkupLine("[bold green]All cycles complete![/]");
        }

        public void DisplayCompletedStats(CompletedStats stats)
        {
            AnsiConsole.MarkupLine($"[italic yellow]Works: [/]{stats.Works} | [italic yellow]Breaks: [/]{stats.Breaks}");
        }

        public void DisplayFinishedMessage()
        {
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

        public void WriteAnsiControl(string code)
        {
            AnsiConsole.Write(new Text(code));
        }

        public void ClearOneLine()
        {
            WriteAnsiControl("\u001b[1A\u001b[2K");
        }

        public void ClearCurrentLine()
        {
            WriteAnsiControl("\u001b[2K");
        }

        public void ClearLines(int count)
        {
            for (int i = 0; i < count; i++)
            {
                ClearOneLine();
            }
        }
    }
}
