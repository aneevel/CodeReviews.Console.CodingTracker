using System;
using System.Collections.Generic;
using System.Text;
using CodeReviews.Console.CodingTracker.aneevel.Models;
using Spectre.Console;

/// <summary>
/// View class responsible for rendering "View All Sessions" menu
/// </summary>
namespace CodeReviews.Console.CodingTracker.aneevel.Views
{
    internal class ViewAllSessionsView
    {
        private readonly Panel _panel = new Panel(new FigletText("Viewing Sessions"))
            .DoubleBorder()
            .BorderColor(Color.Purple)
            .Expand();

        internal void Render(List<CodingSession> sessions)
        {
            AnsiConsole.Write(_panel);
            RenderSessions(sessions);
        }

        private void RenderSessions(List<CodingSession> sessions)
        {
            var table = new Table();
            table.AddColumn(new TableColumn("ID"));
            table.AddColumn(new TableColumn("Start Date"));
            table.AddColumn(new TableColumn("End Date"));
            table.AddColumn(new TableColumn("Duration"));

            if (sessions.Count == 0)
            {
                AnsiConsole.MarkupLine("[red]No sessions found![/]");
            }
            else
            {
                foreach (CodingSession session in sessions)
                {
                    table.AddRow(
                        session.Id.ToString(),
                        session.StartTime.ToString(),
                        session.EndTime.ToString(),
                        session.Duration.ToString()
                    );
                }
                AnsiConsole.Write(table);
            }
        }
    }
}
