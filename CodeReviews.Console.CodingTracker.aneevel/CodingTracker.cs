using CodeReviews.Console.CodingTracker.aneevel.Database;
using CodeReviews.Console.CodingTracker.aneevel.Enums;
using CodeReviews.Console.CodingTracker.aneevel.Models;
using CodeReviews.Console.CodingTracker.aneevel.Services;
using CodeReviews.Console.CodingTracker.aneevel.Views;
using Spectre.Console;

namespace CodeReviews.Console.CodingTracker.aneevel
{
    /// <summary>
    /// Coding Tracker provides the main driver or "engine" for handling the Coding Tracker application
    /// </summary>
    public class CodingTracker
    {
        private readonly string[] _menuOptions =
        [
            "View All Sessions",
            "Start Running Session",
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
            RunningSessionView runningSessionView = new();
            InsertNewSessionView insertNewSessionView = new();
            UpdateSessionView updateSessionView = new();
            DeleteSessionView deleteSessionView = new();

            Run(
                mainMenuView,
                viewAllSessionsView,
                runningSessionView,
                insertNewSessionView,
                updateSessionView,
                deleteSessionView
            );
        }

        private void Run(
            MainMenuView mainMenuView,
            ViewAllSessionsView viewAllSessionsView,
            RunningSessionView runningSessionView,
            InsertNewSessionView insertNewSessionView,
            UpdateSessionView updateSessionView,
            DeleteSessionView deleteSessionView
        )
        {
            while (true)
            {
                mainMenuView.Render();

                string option = UserInputService.GetUserSelection(
                    "Welcome to the [grey]Main Menu.[/] Please select one of the following operations.",
                    _menuOptions
                );

                switch (option)
                {
                    case "View All Sessions":
                        ViewAllSessions(viewAllSessionsView);
                        break;
                    case "Start Running Session":
                        StartRunningSession(runningSessionView);
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
            }
        }

        private void ViewAllSessions(ViewAllSessionsView viewAllSessionsView)
        {
            var sessions = _databaseManager.ReadSessions();
            viewAllSessionsView.Render(sessions);

            UserInputService.GetUserContinue("Press any key to continue...");
        }

        private void StartRunningSession(RunningSessionView runningSessionView)
        {
            runningSessionView.Render();
            CodingSession? session = UserInputService.GetUserRunningCodingSession();

            if (session == null)
            {
                return;
            }

            _databaseManager.InsertRecord(session);

            UserInputService.GetUserContinue(
                "[green]Running session saved![/] Press any key to continue..."
            );
        }

        private void InsertNewSession(InsertNewSessionView insertNewSessionView)
        {
            insertNewSessionView.Render();

            CodingSession session = UserInputService.GetUserCodingSession();

            _databaseManager.InsertRecord(session);

            UserInputService.GetUserContinue(
                "[green]Session created![/] Press any key to continue..."
            );
        }

        private void UpdateSession(UpdateSessionView updateSessionView)
        {
            updateSessionView.Render();

            int id;
            while (true)
            {
                id = UserInputService.GetUserSessionId(
                    "Enter the ID for the session you want to modify:"
                );

                if (_databaseManager.SessionExists(id))
                    break;
                else
                    AnsiConsole.MarkupLine($"[red]Session with ID {id} does not exist![/]");
            }

            CodingSession session = UserInputService.GetUserCodingSession();

            _databaseManager.UpdateSession(
                id,
                session.StartTime,
                session.EndTime,
                session.Duration
            );

            UserInputService.GetUserContinue(
                "[green]Session updated![/] Press any key to continue..."
            );
        }

        private void DeleteSession(DeleteSessionView deleteSessionView)
        {
            deleteSessionView.Render();

            int id;
            while (true)
            {
                id = UserInputService.GetUserSessionId(
                    "Enter the ID of the session you wish to delete:"
                );

                if (_databaseManager.SessionExists(id))
                    break;
                else
                    AnsiConsole.MarkupLine($"[red]Session with ID {id} does not exist![/]");
            }

            if (
                !UserInputService.GetUserConfirmation(
                    $"Are you sure you want to [red]delete[/] session with ID {id}?",
                    "Session will not be deleted. Press any key to return to main menu..."
                )
            )
            {
                return;
            }

            _databaseManager.DeleteSession(id);

            UserInputService.GetUserContinue(
                $"Session with ID {id} successfully [red]deleted![/] Press any key to continue..."
            );
        }
    }
}
