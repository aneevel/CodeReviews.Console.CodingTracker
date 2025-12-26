using System;
using System.Collections.Generic;
using System.Text;
using Spectre.Console;

/// <summary>
/// View class responsible for rendering "View All Sessions" menu
/// </summary>
namespace CodeReviews.Console.CodingTracker.Views
{
    internal class ViewAllSessionsView
    {
        private static readonly Panel panel = new Panel(new FigletText("Viewing Sessions"))
            .DoubleBorder()
            .BorderColor(Color.Purple)
            .Expand();

        internal void Render()
        {
            AnsiConsole.Write(panel);
        }
    }
}
