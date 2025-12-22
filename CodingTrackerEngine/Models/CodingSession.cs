namespace CodingTrackerEngine.Models
{
    /// <summary>
    /// Coding Session provides a data model for tracking start and end times, as well as duration, for a dedicated session of coding
    /// </summary>
    internal class CodingSession
    {
        private readonly int _id;
        public int Id
        {
            get { return _id; }
        }
        private readonly DateTime _startTime;
        public DateTime StartTime
        {
            get { return _startTime; }
        }
        private readonly DateTime _endTime;
        public DateTime EndTime
        {
            get { return _endTime; }
        }
        private readonly TimeSpan _duration;
        public TimeSpan Duration
        {
            get { return _duration; }
        }

        public CodingSession(int id, DateTime startTime, DateTime endTime)
        {
            _id = id;
            this._startTime = startTime;
            this._endTime = endTime;
            this._duration = endTime - startTime;
        }
    }
}
