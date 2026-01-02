using System.Diagnostics;
using System.Globalization;
using CodeReviews.Console.CodingTracker.aneevel.Enums;
using CodeReviews.Console.CodingTracker.aneevel.Extensions;
using CodeReviews.Console.CodingTracker.aneevel.Models;
using Spectre.Console;

namespace CodeReviews.Console.CodingTracker.aneevel.Services
{
    internal static class UserInputService
    {
        private const int PageSize = 10;

        internal static MenuOption GetMenuSelection(string message)
        {
            var input = AnsiConsole.Prompt(
                new SelectionPrompt<MenuOption>()
                    .Title(message)
                    .AddChoices(Enum.GetValues<MenuOption>())
                    .PageSize(PageSize)
                    .MoreChoicesText("[grey](Use arrow keys to see more options)[/]")
                    .UseConverter(option => option.GetDisplayName())
            );

            return input;
        }

        internal static CodingSession GetNewCodingSession()
        {
            while (true)
            {
                var startTime = GetDate(
                    "Enter the Start Time of the session in format MM/dd/yyyy hh:mm AM/PM",
                    "Incorrect format provided; please provide Start Time in the given format."
                );
                var endTime = GetDate(
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

        internal static CodingSession? GetRunningCodingSession()
        {
            AnsiConsole.Clear();

            if (
                !GetConfirmation(
                    "Do you wish to [green]start a new coding session[/]?",
                    "Not starting a new session; press any key to return to main menu"
                )
            )
            {
                return null;
            }

            AnsiConsole.Clear();

            var stopwatch = Stopwatch.StartNew();
            var startTime = DateTime.Now;

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
                        
                        if (!System.Console.KeyAvailable) continue;
                        if (System.Console.ReadKey().Key != ConsoleKey.S) continue;
                        
                        stopwatch.Stop();
                        
                        break;
                    }
                });

            AnsiConsole.Clear();

            var endTime = startTime + stopwatch.Elapsed;

            return new CodingSession
            {
                StartTime = startTime,
                EndTime = endTime,
                Duration = endTime - startTime,
            };
        }

        private static DateTime GetDate(string promptMessage, string invalidInputMessage)
        {
            while (true)
            {
                var dateResponse = AnsiConsole.Ask<string>(promptMessage);

                if (
                    !DateTime.TryParseExact(
                        dateResponse,
                        "MM/dd/yyyy hh:mm tt",
                        new CultureInfo("en-US"),
                        DateTimeStyles.None,
                        out var dateTime
                    )
                )
                {
                    AnsiConsole.WriteLine(invalidInputMessage);
                }
                else
                {
                    return dateTime;
                }
            }
        }

        internal static void GetContinue(string promptMessage)
        {
            AnsiConsole.MarkupLine(promptMessage);
            System.Console.ReadLine();
            AnsiConsole.Clear();
        }

        internal static CodingSession GetExistingCodingSession(
            string promptMessage,
            List<CodingSession> codingSessions
        )
        {
            return AnsiConsole.Prompt(
                new SelectionPrompt<CodingSession>()
                    .Title(promptMessage)
                    .AddChoices(codingSessions.Select(codingSession => codingSession))
                    .PageSize(PageSize)
                    .MoreChoicesText("[grey](Use arrow keys to see more options)[/]")
                    .UseConverter(codingSession =>
                        $"ID: {codingSession.Id} - Start Time: {codingSession.StartTime} - End Time: {codingSession.EndTime}"
                    )
            );
        }

        internal static string GetPageNavigationOption(string promptMessage, List<string> choices)
        {
            return AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title(promptMessage)
                .AddChoices(choices));
        }

        internal static Tuple<SortingField, SortingDirection> GetSortingOptions(string fieldPrompt,
            string directionPrompt, SortingField[] fieldOptions, SortingDirection[] directionOptions)
        {
            var field = GetSortingField(fieldPrompt, fieldOptions);
            var direction = GetSortingDirection(directionPrompt, directionOptions);
            
            return new Tuple<SortingField, SortingDirection>(field, direction);
        }

        private static SortingField GetSortingField(string fieldPrompt, SortingField[] fieldOptions)
        {
            return AnsiConsole.Prompt(
                new SelectionPrompt<SortingField>()
                    .Title(fieldPrompt)
                    .AddChoices(fieldOptions));
        }

        private static SortingDirection GetSortingDirection(string directionPrompt,
            SortingDirection[] directionOptions)
        {
            return AnsiConsole.Prompt(
                new SelectionPrompt<SortingDirection>()
                .Title(directionPrompt)
                .AddChoices(directionOptions));
        }

        internal static bool GetConfirmation(string promptMessage, string unconfirmMessage)
        {
            if (!AnsiConsole.Confirm(promptMessage))
            {
                GetContinue(unconfirmMessage);
                return false;
            }

            AnsiConsole.Clear();
            return true;
        }
        
    }
}
