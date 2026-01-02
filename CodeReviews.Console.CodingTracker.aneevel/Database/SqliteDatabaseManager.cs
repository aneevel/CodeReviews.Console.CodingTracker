using System.Data.SQLite;
using System.Globalization;
using CodeReviews.Console.CodingTracker.aneevel.Handlers;
using CodeReviews.Console.CodingTracker.aneevel.Models;
using Dapper;
using Serilog;

namespace CodeReviews.Console.CodingTracker.aneevel.Database
{
    internal class SqliteDatabaseInitializer : IDatabaseInitializer
    {
        private readonly string _connectionString;

        public SqliteDatabaseInitializer(string connectionString)
        {
            _connectionString = connectionString;
            
            if (InitializeDatabase() == -1)
            {
                Environment.Exit(-1);
            }

            SqlMapper.AddTypeHandler(new TimeSpanHandler());
        }

        public int InitializeDatabase()
        {
            return CreateTable();
        }

        private int CreateTable()
        {
            using var db = new SQLiteConnection(_connectionString);
            const string sql = @"CREATE TABLE IF NOT EXISTS CodingSessions (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT, 
                    StartTime VARCHAR(255) NOT NULL, 
                    EndTime VARCHAR(255) NOT NULL, 
                    Duration VARCHAR(255) NOT NULL
                );";

            try
            {
                db.Execute(sql);
            }
            catch (SQLiteException ex)
            {
                Log.Logger.Fatal(
                    ex,
                    """
                                  Class:{Type} Method: {CreateTableName}\n
                                                          Message: There was an issue creating the database!
                                  """, typeof(SqliteDatabaseManager), nameof(CreateTable)
                );
                return -1;
            }
            catch (Exception ex)
            {
                Log.Logger.Error(
                    ex,
                    """
                                  Class:{Type} Method: {CreateTableName}\n
                                                          Message: There was an explained issue!
                                  """, typeof(SqliteDatabaseManager), nameof(CreateTable)
                );
                return -1;
            }
            return 0;
        }

        public List<CodingSession> ReadSessions()
        {
            using var db = new SQLiteConnection(_connectionString);
            const string sql = "SELECT Id, StartTime, EndTime, Duration FROM CodingSessions";

            List<CodingSession> sessions = [];

            try
            {
                sessions = [.. db.Query<CodingSession>(sql)];
            }
            catch (SQLiteException ex)
            {
                Log.Logger.Error(
                    ex,
                    """
                    Class:{Type} Method: {ReadSessionsName}\n
                                            Message: There was an issue reading sessions!
                    """, typeof(SqliteDatabaseManager), nameof(ReadSessions)
                );
            }
            catch (Exception ex)
            {
                Log.Logger.Error(
                    ex,
                    """
                                  Class:{Type} Method: {ReadSessionsName}\n
                                                          Message: There was an explained issue!
                                  """, typeof(SqliteDatabaseManager), nameof(ReadSessions)
                );
            }

            return sessions;
        }

        public int InsertSession(CodingSession session)
        {
            using var db = new SQLiteConnection(_connectionString);

            const string sql = "INSERT INTO CodingSessions (StartTime, EndTime, Duration) VALUES (@startTime, @endTime, @duration)";
            try
            {
                db.Execute(
                    sql,
                    new
                    {
                        startTime = session.StartTime.ToString(CultureInfo.InvariantCulture),
                        endTime = session.EndTime.ToString(CultureInfo.InvariantCulture),
                        duration = session.Duration.ToString(@"hh\:mm\:ss"),
                    }
                );
            }
            catch (SQLiteException ex)
            {
                Log.Logger.Error(
                    ex,
                    """
                                  Class:{Type} Method: {InsertSessionName}\n
                                                          Message: There was an issue inserting a new session!
                                  """, typeof(SqliteDatabaseManager), nameof(InsertSession)
                );
                return -1;
            }
            catch (Exception ex)
            {
                Log.Logger.Error(
                    ex,
                    """
                                  Class:{Type} Method: {InsertSessionName}\n
                                                          Message: There was an explained issue!
                                  """, typeof(SqliteDatabaseManager), nameof(InsertSession)
                );
                return -1;
            }
            return 0;
        }

        public int UpdateSession(int id, DateTime startTime, DateTime endTime, TimeSpan duration)
        {
            using var db = new SQLiteConnection(_connectionString);
            const string sql = "UPDATE CodingSessions SET StartTime = @startTime, EndTime = @endTime, Duration = @duration WHERE id = @id";
            try
            {
                db.Execute(
                    sql,
                    new
                    {
                        id,
                        startTime = startTime.ToString(CultureInfo.InvariantCulture),
                        endTime = endTime.ToString(CultureInfo.InvariantCulture),
                        duration = duration.ToString(@"hh\:mm\:ss"),
                    }
                );
            }
            catch (SQLiteException ex)
            {
                Log.Logger.Error(
                    ex,
                    """
                                  Class:{Type} Method: {UpdateSessionName}\n
                                                          Message: There was an issue updating an existing session!
                                  """, typeof(SqliteDatabaseManager), nameof(UpdateSession)
                );
                return -1;
            }
            catch (Exception ex)
            {
                Log.Logger.Error(
                    ex,
                    """
                                  Class:{Type} Method: {UpdateSessionName}\n
                                                          Message: There was an explained issue!
                                  """, typeof(SqliteDatabaseManager), nameof(UpdateSession)
                );
            }
            return 0;
        }

        public int DeleteSession(int id)
        {
            using var db = new SQLiteConnection(_connectionString);
            const string sql = "DELETE FROM CodingSessions WHERE id = @id";

            try
            {
                db.Execute(sql, new { id });
            }
            catch (SQLiteException ex)
            {
                Log.Logger.Error(
                    ex,
                    """
                                  Class:{Type} Method: {DeleteSessionName}\n
                                                          Message: There was an issue deleting an existing session!
                                  """, typeof(SqliteDatabaseManager), nameof(DeleteSession)
                );
                return -1;
            }
            catch (Exception ex)
            {
                Log.Logger.Error(
                    ex,
                    """
                                  Class:{Type} Method: {DeleteSessionName}\n
                                                          Message: There was an explained issue!
                                  """, typeof(SqliteDatabaseManager), nameof(DeleteSession)
                );
                return -1;
            }
            return 0;
        }

        public bool SessionExists(int id)
        {
            using var db = new SQLiteConnection(_connectionString);
            const string sql = "SELECT * FROM CodingSessions WHERE id = @id";
            CodingSession? codingSession;

            try
            {
                codingSession = db.QuerySingleOrDefault<CodingSession>(sql, new { id });
            }
            catch (InvalidOperationException ex)
            {
                Log.Logger.Error(
                    ex,
                    """
                                  Class:{Type} Method: {SessionExistsName}\n
                                                          Message: There was an issue checking if a session with id {Id} exists!
                                  """, typeof(SqliteDatabaseManager), nameof(SessionExists), id
                );
                return false;
            }
            catch (Exception ex)
            {
                Log.Logger.Error(
                    ex,
                    """
                                  Class:{Type} Method: {SessionExistsName}\n
                                                          Message: There was an explained issue!
                                  """, typeof(SqliteDatabaseManager), nameof(SessionExists)
                );
                return false;
            }

            return codingSession != null;
        }
    }
}
