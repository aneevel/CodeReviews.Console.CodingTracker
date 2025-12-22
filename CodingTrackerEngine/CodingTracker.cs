using CodingTrackerEngine.Models;
using Spectre.Console;

namespace CodingTrackerEngine
{
    /// <summary>
    /// Coding Tracker provides the main driver or "engine" for handling the Coding Tracker application
    /// </summary>
    public class CodingTracker
    {
        public CodingTracker()
        {
            CodingSession exampleSession = new CodingSession(
                1,
                new DateTime(2025, 12, 22, 12, 0, 0),
                DateTime.Now
            );

            AnsiConsole.MarkupLine(
                $"[blue]Example CodingSession contains parameters;[/]\nId: [green]{exampleSession.Id}[/]\nStart Time: [green]{exampleSession.StartTime}[/]\nEnd Time: [green]{exampleSession.EndTime}[/]\nDuration: [green]{exampleSession.Duration:c}[/]"
            );
        }
    }
}
