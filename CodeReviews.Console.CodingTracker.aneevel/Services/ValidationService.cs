using CodeReviews.Console.CodingTracker.aneevel.Database;

namespace CodeReviews.Console.CodingTracker.aneevel.Services
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
