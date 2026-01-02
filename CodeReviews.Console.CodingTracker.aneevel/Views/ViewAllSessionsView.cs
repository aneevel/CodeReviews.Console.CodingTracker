using CodeReviews.Console.CodingTracker.aneevel.Enums;
using CodeReviews.Console.CodingTracker.aneevel.Extensions;
using CodeReviews.Console.CodingTracker.aneevel.Helpers;
using CodeReviews.Console.CodingTracker.aneevel.Models;
using CodeReviews.Console.CodingTracker.aneevel.Services;
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
                var direction = SortingDirection.Asc;

                while (true)
                {
                    AnsiConsole.Write(_panel);

                    var pagedSessions =
                        codingSessions
                            .SortBy(field, direction)
                            .TakeElementsAtIndex(pageIndex * _pageSize, _pageSize).ToList();

                    AnsiConsole.MarkupLine(
                        $"""
                         Showing sessions {pageIndex * _pageSize} - {pageIndex * _pageSize + 9}
                                                 on page {pageIndex + 1} of {pageCount}
                         """
                    );

                    var table = TableHelper.BuildSessionsTable(pagedSessions);

                    AnsiConsole.Write(table);

                    List<string> choices = ["Exit to Main Menu", "Sort Data"];
                    
                    if (pageIndex > 0)
                    {
                        choices.Add("Previous");
                    }

                    if (pageIndex < pageCount - 1)
                    {
                        choices.Add("Next");
                    }

                    var menuSelection = UserInputService.GetPageNavigationOption("Select option:", choices);

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
                        (field, direction) = UserInputService.GetSortingOptions("Field to Sort On?",
                            "Direction to Sort By?",
                            Enum.GetValues<SortingField>(),
                            Enum.GetValues<SortingDirection>());
                        
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
}
