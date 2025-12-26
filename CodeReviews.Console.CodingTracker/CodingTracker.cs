using System.Globalization;
using CodeReviews.Console.CodingTracker.Database;
using CodeReviews.Console.CodingTracker.Models;
using CodeReviews.Console.CodingTracker.Views;
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
            "Update Session",
            "Delete Session",
            "Exit Application",
        ];

        private readonly SqliteDatabaseManager _databaseManager;

        public CodingTracker(string connectionString)
        {
            _databaseManager = new(connectionString);

            MainMenuView mainMenuView = new();
            ViewAllSessionsView viewAllSessionsView = new();
            InsertNewSessionView insertNewSessionView = new();
            UpdateSessionView updateSessionView = new();
            DeleteSessionView deleteSessionView = new();

            while (true)
            {
                mainMenuView.Render();

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
                        ViewAllSessions(viewAllSessionsView);
                        break;
                    case "Insert Session":
                        InsertNewSession(insertNewSessionView);
                        break;
                    case "Update Session":
                        UpdateSession(updateSessionView);
                        break;
                    case "Delete Session":
                        DeleteSession(deleteSessionView);
                        break;
                    case "Exit Application":
                        Environment.Exit(0);
                        break;
                }
                AnsiConsole.Clear();
            }
        }

        private void ViewAllSessions(ViewAllSessionsView viewAllSessionsView)
        {
            viewAllSessionsView.Render();

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

        private void InsertNewSession(InsertNewSessionView insertNewSessionView)
        {
            insertNewSessionView.Render();

            DateTime startTime;
            DateTime endTime;
            while (true)
            {
                while (true)
                {
                    string startTimeResponse = AnsiConsole.Ask<string>(
                        "Enter the Start Time of the session in format MM/dd/yyyy hh:mm AM/PM:"
                    );

                    if (
                        !DateTime.TryParseExact(
                            startTimeResponse,
                            "MM/dd/yyyy hh:mm tt",
                            new CultureInfo("en-US"),
                            DateTimeStyles.None,
                            out startTime
                        )
                    )
                    {
                        AnsiConsole.WriteLine(
                            "Incorrect format provided; please provide Start Time in the given format."
                        );
                        continue;
                    }
                    break;
                }

                while (true)
                {
                    string endTimeResponse = AnsiConsole.Ask<string>(
                        "Enter the End Time of the session in format MM/dd/yyyy hh:mm AM/PM:"
                    );

                    if (
                        !DateTime.TryParseExact(
                            endTimeResponse,
                            "MM/dd/yyyy hh:mm tt",
                            new CultureInfo("en-US"),
                            DateTimeStyles.None,
                            out endTime
                        )
                    )
                    {
                        AnsiConsole.WriteLine(
                            "Incorrect format provided; please provide End Time in the given format."
                        );
                        continue;
                    }
                    break;
                }

                if (startTime > endTime)
                {
                    AnsiConsole.MarkupLine(
                        "Given Start Time occurs [red]AFTER[/] given End Time; please provide a valid set of dates."
                    );
                    continue;
                }
                break;
            }

            CodingSession session = new()
            {
                StartTime = startTime,
                EndTime = endTime,
                Duration = endTime - startTime,
            };

            _databaseManager.InsertRecord(session);

            AnsiConsole.MarkupLine("[green]Session created![/] Press any key to continue...");
            System.Console.ReadLine();
        }

        private void UpdateSession(UpdateSessionView updateSessionView)
        {
            updateSessionView.Render();

            int id;
            while (true)
            {
                id = AnsiConsole.Ask<int>("Enter the ID for the session you want to modify:");

                if (_databaseManager.SessionExists(id))
                    break;
                else
                    AnsiConsole.MarkupLine($"[red]Session with ID {id} does not exist![/]");
            }

            DateTime startTime;
            DateTime endTime;
            while (true)
            {
                while (true)
                {
                    string startTimeResponse = AnsiConsole.Ask<string>(
                        "Enter the Start Time of the session in format MM/dd/yyyy hh:mm AM/PM:"
                    );

                    if (
                        !DateTime.TryParseExact(
                            startTimeResponse,
                            "MM/dd/yyyy hh:mm tt",
                            new CultureInfo("en-US"),
                            DateTimeStyles.None,
                            out startTime
                        )
                    )
                    {
                        AnsiConsole.WriteLine(
                            "Incorrect format provided; please provide Start Time in the given format."
                        );
                        continue;
                    }
                    break;
                }

                while (true)
                {
                    string endTimeResponse = AnsiConsole.Ask<string>(
                        "Enter the End Time of the session in format MM/dd/yyyy hh:mm AM/PM:"
                    );

                    if (
                        !DateTime.TryParseExact(
                            endTimeResponse,
                            "MM/dd/yyyy hh:mm tt",
                            new CultureInfo("en-US"),
                            DateTimeStyles.None,
                            out endTime
                        )
                    )
                    {
                        AnsiConsole.WriteLine(
                            "Incorrect format provided; please provide End Time in the given format."
                        );
                        continue;
                    }
                    break;
                }

                if (startTime > endTime)
                {
                    AnsiConsole.MarkupLine(
                        "Given Start Time occurs [red]AFTER[/] given End Time; please provide a valid set of dates."
                    );
                    continue;
                }
                break;
            }

            CodingSession session = new()
            {
                StartTime = startTime,
                EndTime = endTime,
                Duration = endTime - startTime,
            };

            _databaseManager.UpdateSession(
                id,
                session.StartTime,
                session.EndTime,
                session.Duration
            );

            AnsiConsole.MarkupLine("[green]Session updated![/] Press any key to continue...");
            System.Console.ReadLine();
        }

        private void DeleteSession(DeleteSessionView deleteSessionView)
        {
            deleteSessionView.Render();

            int id;
            while (true)
            {
                id = AnsiConsole.Ask<int>("Enter the ID of the session you wish to delete:");

                if (_databaseManager.SessionExists(id))
                    break;
                else
                    AnsiConsole.MarkupLine($"[red]Session with ID {id} does not exist![/]");
            }

            if (
                !AnsiConsole.Confirm(
                    $"Are you sure you want to [red]delete[/] session with ID {id}?"
                )
            )
            {
                AnsiConsole.MarkupLine(
                    "Session will not be deleted. Press any key to return to main menu..."
                );
                System.Console.ReadLine();
                return;
            }

            try
            {
                _databaseManager.DeleteSession(id);
            }
            // TODO: Be more specific than this
            catch (Exception ex)
            {
                System.Console.WriteLine(
                    $"Encountered error attempting to delete session with ID {id}: {ex.Message}\nReturning to Main Menu"
                );
                return;
            }

            AnsiConsole.MarkupLine(
                $"Session with ID {id} successfully [red]deleted![/] Press any key to continue..."
            );
            System.Console.ReadLine();
        }
    }
}
