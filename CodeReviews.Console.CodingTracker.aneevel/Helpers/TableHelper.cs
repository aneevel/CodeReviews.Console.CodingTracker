using System.Globalization;
using CodeReviews.Console.CodingTracker.aneevel.Models;
using Spectre.Console;

namespace CodeReviews.Console.CodingTracker.aneevel.Helpers;

internal static class TableHelper
{
   internal static Table BuildSessionsTable(List<CodingSession> sessions)

{
      var table = new Table();

      foreach (var header in (string[])["Id", "Start Time", "End Time", "Duration"])
      {
         table.AddColumn(new TableColumn(header).Centered());
      }

      foreach (var session in sessions)
      {
         table.AddRow(session.Id.ToString(), 
            session.StartTime.ToString(CultureInfo.InvariantCulture), 
            session.EndTime.ToString(CultureInfo.InvariantCulture), 
            session.Duration.ToString(@"hh\:mm\:ss"));
      }

      return table;
   }
}