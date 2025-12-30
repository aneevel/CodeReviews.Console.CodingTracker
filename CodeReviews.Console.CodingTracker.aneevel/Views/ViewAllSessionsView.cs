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

                while (true)
                {
                    var table = new Table();
                    table.AddColumn(new TableColumn("ID"));
                    table.AddColumn(new TableColumn("Start Date"));
                    table.AddColumn(new TableColumn("End Date"));
                    table.AddColumn(new TableColumn("Duration"));

                    AnsiConsole.Write(_panel);
                    List<CodingSession> pagedSessions = codingSessions
                        .Skip(pageIndex * _pageSize)
                        .Take(_pageSize)
                        .ToList();

                    AnsiConsole.MarkupLine(
                        $@"Showing sessions {pageIndex * _pageSize} - {pageIndex * _pageSize + 9}
                        on page {pageIndex + 1} of {pageCount}"
                    );

                    foreach (CodingSession codingSession in pagedSessions)
                    {
                        table.AddRow(
                            codingSession.Id.ToString(),
                            codingSession.StartTime.ToString(),
                            codingSession.EndTime.ToString(),
                            codingSession.Duration.ToString("hh\\:mm\\:ss")
                        );
                    }

                    AnsiConsole.Write(table);

                    var prompt = new SelectionPrompt<string>()
                        .Title("Navigate Pages:")
                        .AddChoices("Exit to Main Menu");

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
                        pageIndex++;
                    else if (menuSelection == "Previous")
                        pageIndex--;
                    else if (menuSelection == "Exit to Main Menu")
                        break;

                    AnsiConsole.Clear();
                }
            }
        }
    }
}
