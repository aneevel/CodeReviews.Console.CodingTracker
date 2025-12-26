using System.Globalization;
using System.Text.Json;
using CodeReviews.Console.CodingTracker.Database;
using CodeReviews.Console.CodingTracker.Models;
using Spectre.Console;

namespace CodeReviews.Console.CodingTracker
{
    /// <summary>
    /// Coding Tracker provides the main driver or "engine" for handling the Coding Tracker application
    /// </summary>
    public class CodingTracker
    {
        private readonly string[] _menuOptions =
        [
            "View All Sessions",
            "Insert Session",
            "Modify Session",
            "Delete Session",
            "Exit Application",
        ];

        private readonly SqliteDatabaseManager _databaseManager;

        public CodingTracker(string connectionString)
        {
            AnsiConsole.MarkupLine(
                $"[blue]Settings set based on configuration file;[/]\nConnection String: [green]{connectionString}[/]"
            );

            _databaseManager = new(connectionString);

            AnsiConsole.MarkupLine(
                "Welcome to [purple]Coding Tracker![/] This amazing application allows you to record, modify, delete, and visualize your coding progress via individualized sessions!"
            );

            while (true)
            {
                var option = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title(
                            "Welcome to the [grey]Main Menu.[/] Please select one of the following operations."
                        )
                        .AddChoices(_menuOptions)
                );

                AnsiConsole.Clear();

                switch (option)
                {
                    case "View All Sessions":
                        ViewAllSessions();
                        break;
                    case "Insert Session":
                        InsertNewSession();
                        break;
                    case "Modify Session":
                        break;
                    case "Delete Session":
                        break;
                    case "Exit Application":
                        Environment.Exit(0);
                        break;
                }
                AnsiConsole.Clear();
            }
        }

        private void ViewAllSessions()
        {
            AnsiConsole.MarkupLine("[yellow]Viewing all sessions...[/]");

            var sessions = _databaseManager.ReadSessions();

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

            AnsiConsole.MarkupLine("Press any key to continue...");
            System.Console.ReadLine();
        }

        private void InsertNewSession()
        {
            AnsiConsole.MarkupLine("[yellow]Inserting new session...[/]");

            string startDateResponse = AnsiConsole.Ask<string>(
                "When did the session begin? Please enter the date and time in format MM/dd/yyyy hh:mm"
            );

            // TODO: Validation

            string endDateResponse = AnsiConsole.Ask<string>(
                "When did the session end? Please enter the date and time in format MM/dd/yyyy hh:mm"
            );

            // TODO: Validation

            CodingSession session = new();
            session.StartTime = DateTime.ParseExact(
                startDateResponse,
                "MM/dd/yyyy hh:mm",
                new CultureInfo("en-US")
            );
            session.EndTime = DateTime.ParseExact(
                endDateResponse,
                "MM/dd/yyyy hh:mm",
                new CultureInfo("en-US")
            );
            session.Duration = new TimeSpan();

            _databaseManager.InsertRecord(session);

            AnsiConsole.MarkupLine("[green]Session created![/] Press any key to continue...");
            System.Console.ReadLine();
        }
    }
}
