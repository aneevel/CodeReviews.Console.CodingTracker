using CodeReviews.Console.CodingTracker.Database;

namespace CodeReviews.Console.CodingTracker.Services
{
    internal class ValidationService
    {
        private readonly SqliteDatabaseManager _databaseManager;

        internal ValidationService(SqliteDatabaseManager databaseManager)
        {
            _databaseManager = databaseManager;
        }

        internal bool IsExistingSessionId(int sessionId)
        {
            return (_databaseManager.SessionExists(sessionId));
        }
    }
}
