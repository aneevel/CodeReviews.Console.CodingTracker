using CodeReviews.Console.CodingTracker.aneevel.Models;

namespace CodeReviews.Console.CodingTracker.aneevel.Database.Repositories;

public interface ICodingSessionRepository
{
    List<CodingSession> GetCodingSessions();
    int InsertCodingSession(CodingSession codingSession);
    int UpdateCodingSession(int id, DateTime startTime, DateTime endTime, TimeSpan duration);
    int DeleteCodingSession(int id);
    bool CodingSessionExists(int id);
}