using CodeReviews.Console.CodingTracker.aneevel.Models;

namespace CodeReviews.Console.CodingTracker.aneevel.Database
{
    internal interface IDatabaseManager
    {
        public abstract List<CodingSession> ReadSessions();

        public abstract int InsertSession(CodingSession session);

        public abstract int UpdateSession(
            int id,
            DateTime startDate,
            DateTime endDate,
            TimeSpan duration
        );

        public abstract int DeleteSession(int id);

        public abstract bool SessionExists(int id);
    }
}
