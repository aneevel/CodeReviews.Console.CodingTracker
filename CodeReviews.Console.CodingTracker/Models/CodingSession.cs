using System.Collections;

namespace CodeReviews.Console.CodingTracker.Models
{
    /// <summary>
    /// Coding Session provides a data model for tracking start and end times, as well as duration, for a dedicated session of coding
    /// </summary>
    internal class CodingSession(int id, DateTime startTime, DateTime endTime)
    {
        public int Id { get; set; } = id;
        public DateTime StartTime { get; set; } = startTime;
        public DateTime EndTime { get; set; } = endTime;
        public TimeSpan Duration { get; set; } = endTime - startTime;
    }
}
