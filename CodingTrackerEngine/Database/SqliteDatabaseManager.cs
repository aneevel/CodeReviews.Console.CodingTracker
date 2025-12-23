using System.Data.SQLite;
using CodingTrackerEngine.Models;
using Dapper;

namespace CodingTrackerEngine.Database
{
    /// <summary>
    /// Concrete implementation of <c>IDatabaseManager</c> that can interface with Sqlite databases.
    /// Contains helper methods for initializing and populating database
    /// </summary>
    internal class SqliteDatabaseManager : IDatabaseManager
    {
        private readonly SQLiteConnection? _connection;

        /// <summary>
        /// Constructs a new <c>SqliteDatabaseManager</c> and connects to database with given name.
        /// Creates one if it doesn't exist.
        /// </summary>
        /// <param name="connectionString">Name of the database file to connect to</param>
        public SqliteDatabaseManager(string connectionString)
        {
            _connection = new SQLiteConnection(connectionString);
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
            using (_connection)
            {
                string sql =
                    "CREATE TABLE IF NOT EXISTS CodingSessions (Id Int PRIMARY KEY AUTO_INCREMENT, StartDate varchar(255) NOT NULL, EndDate varchar(255) NOT NULL, Duration varchar(255) NOT NULL);";
                _connection!.Execute(sql);
            }
        }

        /// <summary>
        /// Read the records from main table
        /// </summary>
        /// <returns>A List of CodingSession records</returns>
        public List<CodingSession> ReadRecords()
        {
            using (_connection)
            {
                string sql = "SELECT * FROM CodingSessions";
                List<CodingSession> sessions = [.. _connection!.Query<CodingSession>(sql)];

                return sessions;
            }
        }

        /// <summary>
        /// Insert a record with the passed parameters
        /// </summary>
        /// <param name="startDate">Start Time of the record</param>
        /// <param name="endDate">End Time of the record</param>
        /// <param name="duration">Duration of record</param>
        public void InsertRecord(DateTime startDate, DateTime endDate, TimeSpan duration)
        {
            using (_connection)
            {
                string sql =
                    "INSERT INTO CodingSessions (StartDate, EndDate, Duration) VALUES (@startDate, @endDate, @duration)";
                _connection!.Execute(
                    sql,
                    new
                    {
                        startDate = startDate.ToString(),
                        endDate = endDate.ToString(),
                        duration = duration.ToString(),
                    }
                );
            }
        }

        /// <summary>
        /// Update a record with the given ID
        /// </summary>
        /// <param name="id">ID of the record</param>
        /// <param name="startDate">Start Time of the record</param>
        /// <param name="endDate">End Time of the record</param>
        /// <param name="duration">Duration of record</param>
        public void UpdateRecord(int id, DateTime startDate, DateTime endDate, TimeSpan duration)
        {
            using (_connection)
            {
                string sql =
                    "UPDATE CodingSessions SET StartDate = @startDate, EndDate = @endDate, Duration = @duration WHERE id = @id";
                _connection!.Execute(
                    sql,
                    new
                    {
                        startDate = startDate.ToString(),
                        endDate = endDate.ToString(),
                        duration = duration.ToString(),
                    }
                );
            }
        }

        /// <summary>
        /// Delete a record with the given ID
        /// </summary>
        /// <param name="id">ID of the record</param>
        public void DeleteRecord(int id)
        {
            using (_connection)
            {
                string sql = "DELETE FROM CodingSessions WHERE id = @id";
                _connection!.Execute(sql, new { id = id });
            }
        }

        /// <summary>
        /// Check if a record exists with given ID
        /// </summary>
        /// <param name="id">Id of the record</param>
        public bool RecordExists(int id)
        {
            using (_connection)
            {
                string sql = "SELECT * FROM CodingSessions WHERE id = @id";
                var session = _connection!.QuerySingleOrDefault<CodingSession>(
                    sql,
                    new { id = id }
                );

                return session != null;
            }
        }

        /// <summary>
        /// Check if the main table in database exists
        /// </summary>
        /// <returns> true if it exists, false otherwise </returns>
        public bool TableExists()
        {
            using (_connection)
            {
                string sql =
                    "SELECT COUNT(*) FROM sqlite_master WHERE type = 'table' AND name = 'CodingSessions'";
                var tables = _connection!.QuerySingleOrDefault(sql);

                return tables != null;
            }
        }
    }
}
