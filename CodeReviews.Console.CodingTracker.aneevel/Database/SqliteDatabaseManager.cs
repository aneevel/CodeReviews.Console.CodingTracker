using System.Data.SQLite;
using CodeReviews.Console.CodingTracker.aneevel.Handlers;
using CodeReviews.Console.CodingTracker.aneevel.Models;
using Dapper;
using Serilog;

namespace CodeReviews.Console.CodingTracker.aneevel.Database
{
    internal class SqliteDatabaseManager : IDatabaseManager
    {
        private readonly string _connectionString;

        public SqliteDatabaseManager(string connectionString)
        {
            _connectionString = connectionString;
            if (Init() == -1)
            {
                Environment.Exit(-1);
            }

            SqlMapper.AddTypeHandler(new TimeSpanHandler());
        }

        private int Init()
        {
            return CreateTable();
        }

        private int CreateTable()
        {
            using var db = new SQLiteConnection(_connectionString);
            string sql =
                @"CREATE TABLE IF NOT EXISTS CodingSessions (
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
                    @$"Class:{typeof(SqliteDatabaseManager)} Method: {nameof(CreateTable)}\n
                        Message: There was an issue creating the database!"
                );
                return -1;
            }
            catch (Exception ex)
            {
                Log.Logger.Error(
                    ex,
                    $@"Class:{typeof(SqliteDatabaseManager)} Method: {nameof(CreateTable)}\n
                        Message: There was an explained issue!"
                );
                return -1;
            }
            return 0;
        }

        public List<CodingSession> ReadSessions()
        {
            using var db = new SQLiteConnection(_connectionString);
            string sql = "SELECT Id, StartTime, EndTime, Duration FROM CodingSessions";

            List<CodingSession> sessions = [];

            try
            {
                sessions = [.. db.Query<CodingSession>(sql)];
            }
            catch (SQLiteException ex)
            {
                Log.Logger.Error(
                    ex,
                    @$"Class:{typeof(SqliteDatabaseManager)} Method: {nameof(ReadSessions)}\n
                        Message: There was an issue reading sessions!"
                );
            }
            catch (Exception ex)
            {
                Log.Logger.Error(
                    ex,
                    $@"Class:{typeof(SqliteDatabaseManager)} Method: {nameof(ReadSessions)}\n
                        Message: There was an explained issue!"
                );
            }

            return sessions;
        }

        public int InsertSession(CodingSession session)
        {
            using var db = new SQLiteConnection(_connectionString);

            string sql =
                "INSERT INTO CodingSessions (StartTime, EndTime, Duration) VALUES (@startTime, @endTime, @duration)";
            try
            {
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
            catch (SQLiteException ex)
            {
                Log.Logger.Error(
                    ex,
                    $@"Class:{typeof(SqliteDatabaseManager)} Method: {nameof(InsertSession)}\n
                        Message: There was an issue inserting a new session!"
                );
                return -1;
            }
            catch (Exception ex)
            {
                Log.Logger.Error(
                    ex,
                    $@"Class:{typeof(SqliteDatabaseManager)} Method: {nameof(InsertSession)}\n
                        Message: There was an explained issue!"
                );
                return -1;
            }
            return 0;
        }

        public int UpdateSession(int id, DateTime startTime, DateTime endTime, TimeSpan duration)
        {
            using var db = new SQLiteConnection(_connectionString);
            string sql =
                "UPDATE CodingSessions SET StartTime = @startTime, EndTime = @endTime, Duration = @duration WHERE id = @id";
            try
            {
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
            catch (SQLiteException ex)
            {
                Log.Logger.Error(
                    ex,
                    $@"Class:{typeof(SqliteDatabaseManager)} Method: {nameof(UpdateSession)}\n
                        Message: There was an issue updating an existing session!"
                );
                return -1;
            }
            catch (Exception ex)
            {
                Log.Logger.Error(
                    ex,
                    $@"Class:{typeof(SqliteDatabaseManager)} Method: {nameof(UpdateSession)}\n
                        Message: There was an explained issue!"
                );
            }
            return 0;
        }

        public int DeleteSession(int id)
        {
            using var db = new SQLiteConnection(_connectionString);
            string sql = "DELETE FROM CodingSessions WHERE id = @id";

            try
            {
                db.Execute(sql, new { id });
            }
            catch (SQLiteException ex)
            {
                Log.Logger.Error(
                    ex,
                    $@"Class:{typeof(SqliteDatabaseManager)} Method: {nameof(DeleteSession)}\n
                        Message: There was an issue deleting an existing session!"
                );
                return -1;
            }
            catch (Exception ex)
            {
                Log.Logger.Error(
                    ex,
                    $@"Class:{typeof(SqliteDatabaseManager)} Method: {nameof(DeleteSession)}\n
                        Message: There was an explained issue!"
                );
                return -1;
            }
            return 0;
        }

        public bool SessionExists(int id)
        {
            using var db = new SQLiteConnection(_connectionString);
            string sql = "SELECT * FROM CodingSessions WHERE id = @id";
            CodingSession? codingSession;

            try
            {
                codingSession = db.QuerySingleOrDefault<CodingSession>(sql, new { id });
            }
            catch (InvalidOperationException ex)
            {
                Log.Logger.Error(
                    ex,
                    $@"Class:{typeof(SqliteDatabaseManager)} Method: {nameof(SessionExists)}\n
                        Message: There was an issue checking if a session with id {id} exists!"
                );
                return false;
            }
            catch (Exception ex)
            {
                Log.Logger.Error(
                    ex,
                    $@"Class:{typeof(SqliteDatabaseManager)} Method: {nameof(SessionExists)}\n
                        Message: There was an explained issue!"
                );
                return false;
            }

            return codingSession != null;
        }
    }
}
