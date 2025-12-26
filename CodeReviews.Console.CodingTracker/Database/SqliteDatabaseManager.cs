using System.Data.SQLite;
using CodeReviews.Console.CodingTracker.Models;
using Dapper;

namespace CodeReviews.Console.CodingTracker.Database
{
    /// <summary>
    /// Concrete implementation of <c>IDatabaseManager</c> that can interface with Sqlite databases.
    /// Contains helper methods for initializing and populating database
    /// </summary>
    internal class SqliteDatabaseManager : IDatabaseManager
    {
        private readonly string _connectionString;

        /// <summary>
        /// Constructs a new <c>SqliteDatabaseManager</c> and connects to database with given name.
        /// Creates one if it doesn't exist.
        /// </summary>
        /// <param name="connectionString">Name of the database file to connect to</param>
        public SqliteDatabaseManager(string connectionString)
        {
            _connectionString = connectionString;
            Init();
        }

        /// <summary>
        /// Sets <c>SqliteDatabaseManager</c> to initial state, creating the needed table if it doesn't exist.
        /// </summary>
        private void Init()
        {
            if (!TableExists())
            {
                CreateTable();
            }
        }

        /// <summary>
        /// Creates the main table (CodingSessions) within the database, given the existing connection
        /// </summary>
        private void CreateTable()
        {
            using var db = new SQLiteConnection(_connectionString);
            string sql =
                "CREATE TABLE IF NOT EXISTS CodingSessions (Id Int AUTO_INCREMENT PRIMARY KEY, StartTime varchar(255) NOT NULL, EndTime varchar(255) NOT NULL, Duration varchar(255) NOT NULL);";
            db.Execute(sql);
        }

        /// <summary>
        /// Read the records from main table
        /// </summary>
        /// <returns>A List of CodingSession records</returns>
        public List<CodingSession> ReadSessions()
        {
            using var db = new SQLiteConnection(_connectionString);
            string sql = "SELECT Id, StartTime, EndTime, Duration FROM CodingSessions";
            List<CodingSession> sessions = db.Query<CodingSession>(sql).ToList();

            return sessions;
        }

        /// <summary>
        /// Insert a session with the passed object
        /// </summary>
        /// <param name="session">Session to insert</param>
        public void InsertRecord(CodingSession session)
        {
            using var db = new SQLiteConnection(_connectionString);
            string sql =
                "INSERT INTO CodingSessions (StartTime, EndTime, Duration) VALUES (@startTime, @endTime, @duration)";
            db.Execute(
                sql,
                new
                {
                    startTime = session.StartTime.ToString(),
                    endTime = session.EndTime.ToString(),
                    duration = session.Duration.ToString(),
                }
            );
        }

        /// <summary>
        /// Update a session with the given ID
        /// </summary>
        /// <param name="id">ID of the session</param>
        /// <param name="startDate">Start Time to update session with</param>
        /// <param name="endDate">End Time to update session with</param>
        /// <param name="duration">Duration to update session with</param>
        public void UpdateSession(int id, DateTime startDate, DateTime endDate, TimeSpan duration)
        {
            using var db = new SQLiteConnection(_connectionString);
            string sql =
                "UPDATE CodingSessions SET StartDate = @startDate, EndDate = @endDate, Duration = @duration WHERE id = @id";
            db.Execute(
                sql,
                new
                {
                    id,
                    startDate = startDate.ToString(),
                    endDate = endDate.ToString(),
                    duration = duration.ToString(),
                }
            );
        }

        /// <summary>
        /// Delete a session with the given ID
        /// </summary>
        /// <param name="id">ID of the session</param>
        public void DeleteSession(int id)
        {
            using var db = new SQLiteConnection(_connectionString);
            string sql = "DELETE FROM CodingSessions WHERE id = @id";
            db.Execute(sql, new { id });
        }

        /// <summary>
        /// Check if a session exists with given ID
        /// </summary>
        /// <param name="id">Id of the session</param>
        public bool SessionExists(int id)
        {
            using var db = new SQLiteConnection(_connectionString);
            string sql = "SELECT * FROM CodingSessions WHERE id = @id";
            var session = db.QuerySingleOrDefault<CodingSession>(sql, new { id });

            return session != null;
        }

        /// <summary>
        /// Check if the main table in database exists
        /// </summary>
        /// <returns> true if it exists, false otherwise </returns>
        public bool TableExists()
        {
            using var db = new SQLiteConnection(_connectionString);
            string sql =
                "SELECT COUNT(*) FROM sqlite_master WHERE type = 'table' AND name = 'CodingSessions'";
            return db.ExecuteScalar<int>(sql) != 0;
        }
    }
}
