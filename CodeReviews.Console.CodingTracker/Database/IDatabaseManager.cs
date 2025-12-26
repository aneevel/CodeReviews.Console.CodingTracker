using CodeReviews.Console.CodingTracker.Models;

namespace CodeReviews.Console.CodingTracker.Database
{
    /// <summary>
    /// Interface <c>IDatabaseManager</c> provides an interface for an object which handles insertion of CodingSessions
    /// </summary>
    internal interface IDatabaseManager
    {
        /// <summary>
        /// Read the sessions from main table
        /// </summary>
        public abstract List<CodingSession> ReadSessions();

        /// <summary>
        /// Insert a record corresponding to session
        /// </summary>
        /// <param name="session">Session being uploaded</param>
        public abstract void InsertRecord(CodingSession session);

        /// <summary>
        /// Update a session with the given ID
        /// </summary>
        /// <param name="id">ID of the session</param>
        /// <param name="startDate">Start Time of the session</param>
        /// <param name="endDate">End Time of the session</param>
        /// <param name="duration">Duration of session</param>
        public abstract void UpdateSession(
            int id,
            DateTime startDate,
            DateTime endDate,
            TimeSpan duration
        );

        /// <summary>
        /// Delete a session with the given ID
        /// </summary>
        /// <param name="id">ID of the session</param>
        public abstract void DeleteSession(int id);

        /// <summary>
        /// Check if a session exists with given ID
        /// </summary>
        /// <param name="id">Id of the session</param>
        public abstract bool SessionExists(int id);

        /// <summary>
        /// Check if the main table in database exists
        /// </summary>
        /// <returns> true if it exists, false otherwise </returns>
        public abstract bool TableExists();
    }
}
