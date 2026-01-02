using System.Globalization;
using CodeReviews.Console.CodingTracker.aneevel.Extensions;
using CodeReviews.Console.CodingTracker.aneevel.Models;
using Spectre.Console;

namespace CodeReviews.Console.CodingTracker.aneevel.Views
{
    internal class ViewAllSessionsView
    {
        private readonly int _pageSize = 10;
        private readonly Panel _panel = new Panel(new FigletText("Viewing Sessions"))
            .DoubleBorder()
            .BorderColor(Color.Purple)
            .Expand();

        internal void Render(List<CodingSession> codingSessions)
        {
            RenderSessions(codingSessions);
        }

        private void RenderSessions(List<CodingSession> codingSessions)
        {
            if (codingSessions.Count == 0)
            {
                AnsiConsole.MarkupLine("[red]No sessions found![/]");
            }
            else
            {
                var pageIndex = 0;
                var pageCount = (int)Math.Ceiling(codingSessions.Count / (double)_pageSize);

                var field = SortingField.Id;
                var direction = SortingDirection.ASC;

                while (true)
                {
                    var table = new Table();
                    table.AddColumn(new TableColumn("ID"));
                    table.AddColumn(new TableColumn("Start Date"));
                    table.AddColumn(new TableColumn("End Date"));
                    table.AddColumn(new TableColumn("Duration"));

                    AnsiConsole.Write(_panel);

                    var pagedSessions =
                        codingSessions
                            .SortBy(field, direction)
                            .TakeElementsAtIndex(pageIndex * _pageSize, _pageSize).ToList();

                    pagedSessions = [.. pagedSessions.Skip(pageIndex * _pageSize).Take(_pageSize)];

                    AnsiConsole.MarkupLine(
                        $"""
                         Showing sessions {pageIndex * _pageSize} - {pageIndex * _pageSize + 9}
                                                 on page {pageIndex + 1} of {pageCount}
                         """
                    );

                    foreach (var codingSession in pagedSessions)
                    {
                        table.AddRow(
                            codingSession.Id.ToString(),
                            codingSession.StartTime.ToString(CultureInfo.InvariantCulture),
                            codingSession.EndTime.ToString(CultureInfo.InvariantCulture),
                            codingSession.Duration.ToString(@"hh\:mm\:ss")
                        );
                    }

                    AnsiConsole.Write(table);

                    var prompt = new SelectionPrompt<string>()
                        .Title("Navigate Pages:")
                        .AddChoices(["Exit to Main Menu", "Sort Data"]);

                    if (pageIndex > 0)
                    {
                        prompt.AddChoices("Previous");
                    }

                    if (pageIndex < pageCount - 1)
                    {
                        prompt.AddChoices("Next");
                    }

                    var menuSelection = AnsiConsole.Prompt(prompt);

                    if (menuSelection == "Next")
                    {
                        pageIndex++;
                        AnsiConsole.Clear();
                    }
                    else if (menuSelection == "Previous")
                    {
                        pageIndex--;
                        AnsiConsole.Clear();
                    }
                    else if (menuSelection == "Sort Data")
                    {
                        field = AnsiConsole.Prompt(
                            new SelectionPrompt<SortingField>()
                                .Title("Field to Sort On?")
                                .AddChoices(Enum.GetValues<SortingField>())
                        );

                        direction = AnsiConsole.Prompt(
                            new SelectionPrompt<SortingDirection>()
                                .Title("Direction to Sort By?")
                                .AddChoices(Enum.GetValues<SortingDirection>())
                        );
                        AnsiConsole.Clear();
                        pageIndex = 0;
                    }
                    else if (menuSelection == "Exit to Main Menu")
                    {
                        AnsiConsole.Clear();
                        break;
                    }
                }
            }
        }
    }

    public enum SortingField
    {
        Id,
        StartTime,
        EndTime,
        Duration
    }

    public enum SortingDirection
    {
        ASC,
        DSC
    }
}
