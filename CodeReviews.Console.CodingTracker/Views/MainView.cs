using Spectre.Console;

namespace CodeReviews.Console.CodingTracker.Views
{
    /// <summary>
    /// View class responsible for rendering the "Main Menu"
    /// </summary>
    internal class MainView
    {
        private static readonly Panel panel = new Panel(new FigletText("Coding Tracker").Centered())
            .DoubleBorder()
            .BorderColor(Color.Purple)
            .Expand();

        internal void Render()
        {
            AnsiConsole.Write(panel);
        }
    }
}
