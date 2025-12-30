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

                MenuOption option = UserInputService.GetUserSelection(
                    "Welcome to the [grey]Main Menu.[/] Please select one of the following operations."
                );

                AnsiConsole.Clear();

                switch (option)
                {
                    case MenuOption.ViewAllSessions:
                        ViewAllSessions(viewAllSessionsView);
                        break;
                    case MenuOption.StartRunningSession:
                        StartRunningSession(runningSessionView);
                        break;
                    case MenuOption.InsertSession:
                        InsertNewSession(insertNewSessionView);
                        break;
                    case MenuOption.UpdateSession:
                        UpdateSession(updateSessionView);
                        break;
                    case MenuOption.DeleteSession:
                        DeleteSession(deleteSessionView);
                        break;
                    case MenuOption.ExitApplication:
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
                $"[green]Running session saved![/] Your session lasted [green]{session.Duration.Hours} hours, {session.Duration.Minutes} minutes, and {session.Duration.Seconds} seconds[/]! Press any key to continue..."
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
