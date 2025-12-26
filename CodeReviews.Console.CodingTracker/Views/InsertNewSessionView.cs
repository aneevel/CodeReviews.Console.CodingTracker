using Spectre.Console;

namespace CodeReviews.Console.CodingTracker.Views
{
    /// <summary>
    /// View class responsible for rendering the "Insert New Session" menu
    /// </summary>
    internal class InsertNewSessionView
    {
        private readonly Panel _panel = new Panel(new FigletText("Insert Session").Centered())
            .DoubleBorder()
            .BorderColor(Color.Purple)
            .Expand();

        internal void Render() => AnsiConsole.Write(_panel);
    }
}
