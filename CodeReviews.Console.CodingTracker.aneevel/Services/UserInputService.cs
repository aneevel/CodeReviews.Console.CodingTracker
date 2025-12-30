using System.Diagnostics;
using System.Globalization;
using CodeReviews.Console.CodingTracker.aneevel.Enums;
using CodeReviews.Console.CodingTracker.aneevel.Extensions;
using CodeReviews.Console.CodingTracker.aneevel.Models;
using Spectre.Console;

namespace CodeReviews.Console.CodingTracker.aneevel.Services
{
    internal class UserInputService
    {
        internal static MenuOption GetUserSelection(string message)
        {
            MenuOption input = AnsiConsole.Prompt(
                new SelectionPrompt<MenuOption>()
                    .Title(message)
                    .AddChoices(Enum.GetValues<MenuOption>())
                    .UseConverter(option => option.GetDisplayName())
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

            AnsiConsole.Clear();

            Stopwatch stopwatch = Stopwatch.StartNew();
            DateTime startTime = DateTime.Now;

            var layout = new Layout("Root").SplitRows(new Layout("Top"), new Layout("Bottom"));

            layout["Top"]
                .Update(
                    Align.Center(
                        new Panel(
                            "[blue]Coding session in progress...[/]\nEnter [blue]'s'[/] to end the session."
                        ).BorderColor(Color.Cyan)
                    )
                )
                .Ratio(1);

            layout["Bottom"]
                .Update(
                    Align.Center(new Panel(new Markup("[blue]00:00:00[/]")).BorderColor(Color.Cyan))
                )
                .Ratio(2);

            AnsiConsole
                .Live(layout)
                .AutoClear(true)
                .Start(ctx =>
                {
                    while (true)
                    {
                        var elapsed = stopwatch.Elapsed;

                        layout["Bottom"]
                            .Update(
                                Align.Center(
                                    new Panel(
                                        new Markup($"[blue]{elapsed:hh\\:mm\\:ss}[/]")
                                    ).BorderColor(Color.Cyan)
                                )
                            )
                            .Ratio(2);

                        ctx.Refresh();
                        if (System.Console.KeyAvailable)
                        {
                            if (System.Console.ReadKey().Key == ConsoleKey.S)
                            {
                                stopwatch.Stop();
                                break;
                            }
                        }
                    }
                });

            AnsiConsole.Clear();

            DateTime endTime = startTime + stopwatch.Elapsed;

            return new()
            {
                StartTime = startTime,
                EndTime = endTime,
                Duration = endTime - startTime,
            };
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
