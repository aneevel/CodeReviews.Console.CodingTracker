using System.Globalization;
using System.Data.SQLite;
using CodeReviews.Console.CodingTracker.aneevel.Models;
using Dapper;
using Serilog;

namespace CodeReviews.Console.CodingTracker.aneevel.Database.Repositories;

public class CodingSessionRepository(string connectionString) : ICodingSessionRepository
{
    public List<CodingSession> GetCodingSessions()
    {
       using var db = new SQLiteConnection(connectionString);
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
               Class:{Type} Method: {GetCodingSessions}\n
                                       Message: There was an issue reading sessions!
               """, typeof(SqliteDatabaseInitializer), nameof(GetCodingSessions)
           );
       }
       catch (Exception ex)
       {
           Log.Logger.Error(
               ex,
               """
               Class:{Type} Method: {GetCodingSessions}\n
                                       Message: There was an explained issue!
               """, typeof(CodingSessionRepository), nameof(GetCodingSessions)
           );
       }

       return sessions;
    }

    public int InsertCodingSession(CodingSession codingSession)
    {
       using var db = new SQLiteConnection(connectionString);

            const string sql = "INSERT INTO CodingSessions (StartTime, EndTime, Duration) VALUES (@startTime, @endTime, @duration)";
            try
            {
                db.Execute(
                    sql,
                    new
                    {
                        startTime = codingSession.StartTime.ToString(CultureInfo.InvariantCulture),
                        endTime = codingSession.EndTime.ToString(CultureInfo.InvariantCulture),
                        duration = codingSession.Duration.ToString(@"hh\:mm\:ss"),
                    }
                );
            }
            catch (SQLiteException ex)
            {
                Log.Logger.Error(
                    ex,
                    """
                                  Class:{Type} Method: {InsertCodingSessionName}\n
                                                          Message: There was an issue inserting a new session!
                                  """, typeof(CodingSessionRepository), nameof(InsertCodingSession)
                );
                return -1;
            }
            catch (Exception ex)
            {
                Log.Logger.Error(
                    ex,
                    """
                                  Class:{Type} Method: {InsertCodingSessionName}\n
                                                          Message: There was an explained issue!
                                  """, typeof(CodingSessionRepository), nameof(InsertCodingSession)
                );
                return -1;
            }
            return 0;
    }

    public int UpdateCodingSession(int id, DateTime startTime, DateTime endTime, TimeSpan duration)
    {
       using var db = new SQLiteConnection(connectionString);
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
               Class:{Type} Method: {UpdateCodingSession}\n
                                       Message: There was an issue updating an existing session!
               """, typeof(CodingSessionRepository), nameof(UpdateCodingSession)
           );
           return -1;
       }
       catch (Exception ex)
       {
           Log.Logger.Error(
               ex,
               """
               Class:{Type} Method: {UpdateCodingSession}\n
                                       Message: There was an explained issue!
               """, typeof(CodingSessionRepository), nameof(UpdateCodingSession)
           );
       }
       return 0;
    }

    public int DeleteCodingSession(int id)
    {
       using var db = new SQLiteConnection(connectionString);
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
               Class:{Type} Method: {DeleteCodingSession}\n
                                       Message: There was an issue deleting an existing session!
               """, typeof(CodingSessionRepository), nameof(DeleteCodingSession)
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
               """, typeof(CodingSessionRepository), nameof(DeleteCodingSession)
           );
           return -1;
       }
       return 0;
    }

    public bool CodingSessionExists(int id)
    {
       using var db = new SQLiteConnection(connectionString);
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
               Class:{Type} Method: {CodingSessionExists}\n
                                       Message: There was an issue checking if a session with id {Id} exists!
               """, typeof(CodingSessionRepository), nameof(CodingSessionExists), id
           );
           return false;
       }
       catch (Exception ex)
       {
           Log.Logger.Error(
               ex,
               """
               Class:{Type} Method: {CodingSessionExists}\n
                                       Message: There was an explained issue!
               """, typeof(CodingSessionRepository), nameof(CodingSessionExists)
           );
           return false;
       }

       return codingSession != null;
    }
}