using System.Globalization;

namespace CodeReviews.Console.CodingTracker.aneevel.Models
{
    /// <summary>
    /// Coding Session provides a data model for tracking start and end times, as well as duration, for a dedicated session of coding
    /// </summary>
    public class CodingSession
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public TimeSpan Duration { get; set; }
    }
}
