using Spectre.Console;

namespace CodeReviews.Console.CodingTracker.aneevel.Views
{
    /// <summary>
    /// View class responsible for rendering the "Update Session" menu
    /// </summary>
    internal class UpdateSessionView
    {
        private readonly Panel _panel = new Panel(new FigletText("Update Session").Centered())
            .DoubleBorder()
            .BorderColor(Color.Purple)
            .Expand();

        internal void Render() => AnsiConsole.Write(_panel);
    }
}
