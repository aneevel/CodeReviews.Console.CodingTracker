using System.Data.SQLite;
using CodeReviews.Console.CodingTracker.aneevel.Handlers;
using CodeReviews.Console.CodingTracker.aneevel.Models;
using Dapper;

namespace CodeReviews.Console.CodingTracker.aneevel.Database
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
        /// Adds relevant Type Handlers to help out the reflection system in Dapper
        /// </summary>
        /// <param name="connectionString">Name of the database file to connect to</param>
        public SqliteDatabaseManager(string connectionString)
        {
            _connectionString = connectionString;
            Init();

            SqlMapper.AddTypeHandler(new TimeSpanHandler());
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
                "CREATE TABLE IF NOT EXISTS CodingSessions (Id INTEGER PRIMARY KEY AUTOINCREMENT, StartTime VARCHAR(255) NOT NULL, EndTime VARCHAR(255) NOT NULL, Duration VARCHAR(255) NOT NULL);";
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

            List<CodingSession> sessions = [];

            try
            {
                sessions = [.. db.Query<CodingSession>(sql)];
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Unable to read sessions due to error: {ex.Message}");
            }

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
        /// <param name="startTime">Start Time to update session with</param>
        /// <param name="endTime">End Time to update session with</param>
        /// <param name="duration">Duration to update session with</param>
        public void UpdateSession(int id, DateTime startTime, DateTime endTime, TimeSpan duration)
        {
            using var db = new SQLiteConnection(_connectionString);
            string sql =
                "UPDATE CodingSessions SET StartTime = @startTime, EndTime = @endTime, Duration = @duration WHERE id = @id";
            db.Execute(
                sql,
                new
                {
                    id,
                    startTime = startTime.ToString(),
                    endTime = endTime.ToString(),
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
