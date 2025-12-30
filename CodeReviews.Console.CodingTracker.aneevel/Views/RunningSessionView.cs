using Spectre.Console;

namespace CodeReviews.Console.CodingTracker.aneevel.Views
{
    /// <summary>
    /// View class responsible for rendering the "Running Session" menu
    /// </summary>
    internal class RunningSessionView
    {
        private readonly Panel _panel = new Panel(
            new FigletText("Start Running Session").Centered()
        )
            .DoubleBorder()
            .BorderColor(Color.Purple)
            .Expand();

        internal void Render() => AnsiConsole.Write(_panel);
    }
}
