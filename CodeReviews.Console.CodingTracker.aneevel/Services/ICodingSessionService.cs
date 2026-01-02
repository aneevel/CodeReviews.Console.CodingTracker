using CodeReviews.Console.CodingTracker.aneevel.Models;

namespace CodeReviews.Console.CodingTracker.aneevel.Services;

public interface ICodingSessionService
{
    List<CodingSession> GetCodingSessions();    
    int InsertCodingSession(CodingSession codingSession);
    int UpdateCodingSession(int id, DateTime startTime, DateTime endTime, TimeSpan duration);
    int DeleteCodingSession(int id);
}