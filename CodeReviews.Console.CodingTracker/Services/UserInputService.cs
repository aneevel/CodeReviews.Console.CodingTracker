using System.Diagnostics;
using System.Globalization;
using CodeReviews.Console.CodingTracker.Models;
using Spectre.Console;

namespace CodeReviews.Console.CodingTracker.Services
{
    internal class UserInputService
    {
        internal static string GetUserSelection(string message, string[] options)
        {
            string input = AnsiConsole.Prompt(
                new SelectionPrompt<string>().Title(message).AddChoices(options)
            );

            return input;
        }

        internal static CodingSession GetUserCodingSession()
        {
            while (true)
            {
                DateTime startTime = GetUserDate(
                    "Enter the Start Time of the session in format MM/dd/yyyy hh:mm AM/PM",
                    "Incorrect format provided; please provide Start Time in the given format."
                );
                DateTime endTime = GetUserDate(
                    "Enter the End Time of the session in format MM/dd/yyyy hh:mm AM/PM:",
                    "Incorrect format provided; please provide End Time in the given format."
                );

                if (startTime > endTime)
                {
                    AnsiConsole.MarkupLine(
                        "Given Start Time occurs [red]AFTER[/] given End Time; please provide a valid set of dates."
                    );
                    continue;
                }

                CodingSession session = new()
                {
                    StartTime = startTime,
                    EndTime = endTime,
                    Duration = endTime - startTime,
                };

                return session;
            }
        }

        internal static CodingSession? GetUserRunningCodingSession()
        {
            AnsiConsole.Clear();

            if (
                !GetUserConfirmation(
                    "Do you wish to [green]start a new coding session[/]?",
                    "Not starting a new session; press any key to return to main menu"
                )
            )
            {
                return null;
            }

            Stopwatch stopwatch = Stopwatch.StartNew();

            while (true)
            {
                AnsiConsole.Clear();
                AnsiConsole.MarkupLine("[blue]Coding session in progress...[/]");
                AnsiConsole.MarkupLine("Enter [blue]any key[/] to end the session.");

                var elapsed = stopwatch.Elapsed;
                var stopwatchDisplay = new Markup($"[blue]{elapsed:hh\\:mm\\:ss}[/]");
                var stopwatchPanel = new Panel(stopwatchDisplay)
                    .BorderColor(Color.Cyan)
                    .RoundedBorder();
                AnsiConsole.Write(stopwatchPanel);
                Thread.Sleep(1000);
            }

            return new CodingSession();
        }

        private static DateTime GetUserDate(string promptMessage, string invalidInputMessage)
        {
            while (true)
            {
                string dateResponse = AnsiConsole.Ask<string>(promptMessage);

                if (
                    !DateTime.TryParseExact(
                        dateResponse,
                        "MM/dd/yyyy hh:mm tt",
                        new CultureInfo("en-US"),
                        DateTimeStyles.None,
                        out DateTime dateTime
                    )
                )
                {
                    AnsiConsole.WriteLine(invalidInputMessage);
                    continue;
                }
                else
                {
                    return dateTime;
                }
            }
        }

        internal static void GetUserContinue(string promptMessage)
        {
            AnsiConsole.MarkupLine(promptMessage);
            System.Console.ReadLine();
            AnsiConsole.Clear();
        }

        internal static int GetUserSessionId(string promptMessage)
        {
            return AnsiConsole.Ask<int>(promptMessage);
        }

        internal static bool GetUserConfirmation(string promptMessage, string unconfirmMessage)
        {
            if (!AnsiConsole.Confirm(promptMessage))
            {
                GetUserContinue(unconfirmMessage);
                return false;
            }

            AnsiConsole.Clear();
            return true;
        }
    }
}
