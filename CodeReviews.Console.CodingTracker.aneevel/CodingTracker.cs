using CodeReviews.Console.CodingTracker.aneevel.Database;
using CodeReviews.Console.CodingTracker.aneevel.Database.Repositories;
using CodeReviews.Console.CodingTracker.aneevel.Enums;
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
        private readonly SqliteDatabaseInitializer _databaseInitializer;
        private readonly CodingSessionRepository _codingSessionRepository;
        private readonly CodingSessionService _codingSessionService;

        public CodingTracker(string connectionString)
        {
            _databaseInitializer = new SqliteDatabaseInitializer(connectionString);

            MainMenuView mainMenuView = new();
            ViewAllSessionsView viewAllSessionsView = new();
            RunningSessionView runningSessionView = new();
            InsertNewSessionView insertNewSessionView = new();
            UpdateSessionView updateSessionView = new();
            DeleteSessionView deleteSessionView = new();

            _codingSessionRepository = new CodingSessionRepository(connectionString);
            _codingSessionService = new CodingSessionService(_codingSessionRepository);  

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

                var option = UserInputService.GetMenuSelection(
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
                    default:
                        throw new InvalidOperationException("Unknown Menu Option provided!");
                }
            }
        }

        private void ViewAllSessions(ViewAllSessionsView viewAllSessionsView)
        {
            var sessions = _codingSessionService.GetCodingSessions();
            viewAllSessionsView.Render(sessions);
        }

        private void StartRunningSession(RunningSessionView runningSessionView)
        {
            runningSessionView.Render();
            var session = UserInputService.GetRunningCodingSession();

            if (session == null)
            {
                return;
            }

            if (_codingSessionService.InsertCodingSession(session) == 0)
            {
                UserInputService.GetContinue(
                    $"[green]Running session saved![/] Your session lasted [green]{session.Duration.Hours} hours, {session.Duration.Minutes} minutes, and {session.Duration.Seconds} seconds[/]! Press any key to continue..."
                );
            }
        }

        private void InsertNewSession(InsertNewSessionView insertNewSessionView)
        {
            insertNewSessionView.Render();

            var session = UserInputService.GetNewCodingSession();

            if (_codingSessionService.InsertCodingSession(session) == 0)
            {
                UserInputService.GetContinue(
                    "[green]Session created![/] Press any key to continue..."
                );
            }
        }

        private void UpdateSession(UpdateSessionView updateSessionView)
        {
            updateSessionView.Render();

            var selectedSession = UserInputService.GetExistingCodingSession(
                "Select the session you wish to modify:",
                _codingSessionService.GetCodingSessions()
            );

            if (
                !UserInputService.GetConfirmation(
                    $"Are you sure you want to [blue]modify[/] session with ID {selectedSession.Id}?",
                    "Session will not be modified. Press any key to return to main menu..."
                )
            )
            {
                return;
            }

            var updatedSession = UserInputService.GetNewCodingSession();
            updatedSession.Id = selectedSession.Id;

            if (
                _codingSessionService.UpdateCodingSession(
                    updatedSession.Id,
                    updatedSession.StartTime,
                    updatedSession.EndTime,
                    updatedSession.Duration
                ) == 0
            )
            {
                UserInputService.GetContinue(
                    "[green]Session updated![/] Press any key to continue..."
                );
            }
        }

        private void DeleteSession(DeleteSessionView deleteSessionView)
        {
            deleteSessionView.Render();

            var selectedSession = UserInputService.GetExistingCodingSession(
                "Select the session you wish to delete:",
                _codingSessionService.GetCodingSessions()
            );

            if (
                !UserInputService.GetConfirmation(
                    $"Are you sure you want to [red]delete[/] session with ID {selectedSession.Id}?",
                    "Session will not be deleted. Press any key to return to main menu..."
                )
            )
            {
                return;
            }

            if (_codingSessionService.DeleteCodingSession(selectedSession.Id) == 0)
            {
                UserInputService.GetContinue(
                    $"Session with ID {selectedSession.Id} successfully [red]deleted![/] Press any key to continue..."
                );
            }
        }
    }
}
