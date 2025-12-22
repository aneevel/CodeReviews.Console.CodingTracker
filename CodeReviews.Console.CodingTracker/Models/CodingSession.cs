using System;
using System.Collections.Generic;
using System.Text;

namespace CodeReviews.Console.CodingTracker.Models
{
    internal class CodingSession
    {
        private readonly int _id;
        public int Id
        {
            get { return _id; }
        }
        private DateTime startTime { get; set; }
        private DateTime endTime { get; set; }
        private TimeSpan duration { get; set; }

        public CodingSession(int id, DateTime startTime, DateTime endTime, TimeSpan duration)
        {
            _id = id;
            this.startTime = startTime;
            this.endTime = endTime;
            this.duration = duration;
        }
    }
}
