using Spectre.Console;

namespace CodeReviews.Console.CodingTracker.aneevel.Views
{
    /// <summary>
    /// View class responsible for rendering the "Delete Session" menu
    /// </summary>
    internal class DeleteSessionView
    {
        private readonly Panel _panel = new Panel(new FigletText("Delete Session").Centered())
            .DoubleBorder()
            .BorderColor(Color.Purple)
            .Expand();

        internal void Render() => AnsiConsole.Write(_panel);
    }
}
